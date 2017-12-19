using Fighting.Worker.Abstractions;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Fighting.Worker
{

    /// <summary>
    /// A server for executing pipeline jobs
    /// </summary>
    public class WorkerServer : IWorkerServer
    {
        private readonly ILogger<WorkerServer> _logger;

        private readonly IWorkerStorage _pipelineStorage;
        private readonly IWorkerTaskFactory _taskFactory;

        public WorkerServer(IWorkerStorage pipelineStorage, IWorkerTaskFactory taskFactory, ILogger<WorkerServer> logger)
        {
            _pipelineStorage = pipelineStorage ?? throw new ArgumentNullException(nameof(pipelineStorage));
            _taskFactory = taskFactory ?? throw new ArgumentNullException(nameof(taskFactory));
            _logger = logger;
        }

        public WorkerServer(IWorkerStorage pipelineStorage, ILogger<WorkerServer> logger)
            : this(pipelineStorage, new ReflectionWorkerTaskFactory(Assembly.GetCallingAssembly()), logger)
        {
        }

        /// <summary>
        /// This method should be called by a Hangfire client to execute a job
        /// </summary>
        [DisplayName("{0}")]
        public void ExecuteJob(string jobContextId, IJobCancellationToken jct)
        {
            _logger.LogInformation("Begin job '{0}'", jobContextId);
            // Setup cancellation tokens
            var cts = CancellationTokenSource.CreateLinkedTokenSource(jct.ShutdownToken);
            var ct = cts.Token;
            // Read job context from storage
            _logger.LogDebug("Getting context for job '{0}'", jobContextId);
            var jobContext = GetJobContextAsync(jobContextId, ct).Result;
            if (jobContext == null)
                throw new NullReferenceException($"Missing {nameof(jobContext)}");
            jobContext.Start = DateTime.UtcNow;
            UpdateJobContextAsync(jobContext, ct).Wait();
            // Prepare the task queue
            var taskExecutions = new List<Task>();
            var queue = GetConcurrentQueue(jobContext);
            _logger.LogDebug("Job '{0}' has '{1}' tasks", jobContext.Id, jobContext.Queue.Count());
            var syncLock = new object();
            // Begin dependency scope
            using (GetJobDependencyScope())
            {
                // Iterate over the task queue 
                while (queue.TryDequeue(out IWorkerTaskContext taskContext))
                {
                    if (string.IsNullOrWhiteSpace(taskContext.Task))
                        throw new InvalidOperationException("Task context does not have a task name");
                    if (string.IsNullOrWhiteSpace(taskContext.Id))
                        throw new InvalidOperationException($"Task '{taskContext.Task}' does not have an ID");
                    _logger.LogInformation("Begin task '{0}'", taskContext.Id);
                    // Check cancellation tokens
                    CheckForCancellation(jct, cts);
                    // Check if task already executed
                    if (taskContext.End > DateTime.MinValue)
                    {
                        _logger.LogInformation("Task '{0}' already executed, skipping...", taskContext.Id);
                        continue;
                    }
                    // If not parallel block thread with WaitAll until previous tasks complete
                    if (!taskContext.RunParallel)
                    {
                        _logger.LogDebug("Task '{0}' is not parallel, waiting for previous tasks to complete...",
                            taskContext.Id);
                        Task.WaitAll(taskExecutions.ToArray(), ct);
                    }
                    // Start task
                    taskContext.Start = DateTime.UtcNow;
                    OnTaskStarted(jobContext, taskContext);
                    // Execute task
                    _logger.LogDebug("Create instance of '{0}' for task '{1}'", taskContext.Task,
                        taskContext.Id);
                    var taskInstance = CreateTaskInstance(taskContext);
                    OnTaskInstanceCreated(jobContext, taskContext, taskInstance);
                    _logger.LogDebug("Execute task '{0}'", taskContext.Id);
                    var taskExecution = ExecuteTaskAsync(taskInstance, taskContext, jobContext, ct);
                    var taskContinuation = taskExecution.ContinueWith(continuation =>
                    {
                        // Get result
                        var innerTaskContext = continuation.Result;
                        // Release instance
                        _logger.LogDebug("Releasing instance of '{0}' for task '{1}'",
                            innerTaskContext.Task, innerTaskContext.Id);
                        ReleaseTaskInstance(jobContext, innerTaskContext, taskInstance);
                        // Check for task exception
                        if (continuation.Exception != null)
                            throw continuation.Exception.GetBaseException();
                        _logger.LogInformation("Finished task '{0}'", innerTaskContext.Id);
                        OnTaskExecuted(jobContext, innerTaskContext, taskInstance);
                        _logger.LogDebug("Update job '{0}' with task '{1}'",
                            jobContext.Id, innerTaskContext.Id);
                        // Update job context with results of task
                        lock (syncLock)
                        {
                            innerTaskContext.End = DateTime.UtcNow;
                            jobContext.AddCompletedTask(innerTaskContext);
                            jobContext = UpdateJobContextForTaskAsync(jobContext,
                                innerTaskContext, ct).Result;
                        }
                        OnJobContextUpdatedForTask(jobContext, innerTaskContext, taskInstance);
                    }, ct);
                    // Marshal tasks back to the worker thread
                    taskExecution.ConfigureAwait(true);
                    taskContinuation.ConfigureAwait(true);
                    taskExecutions.Add(taskExecution);
                    taskExecutions.Add(taskContinuation);
                    // If not parallel then block thread with Wait
                    if (!taskContext.RunParallel)
                    {
                        _logger.LogDebug("Task '{0}' is not parallel, waiting for completion...",
                            taskContext.Id);
                        taskExecution.Wait(cts.Token);
                    }
                }
                Task.WaitAll(taskExecutions.ToArray(), cts.Token);
            }
            // Job completion
            jobContext.End = DateTime.UtcNow;
            UpdateJobContextAsync(jobContext, ct).Wait();
            _logger.LogInformation("Finished job '{0}'", jobContext.Id);
        }

        #region Job context

        /// <summary>
        /// Converts the pipeline job queue into a concurrent queue
        /// </summary>
        protected virtual ConcurrentQueue<IWorkerTaskContext> GetConcurrentQueue(
            IWorkerContext jobContext)
        {
            var orderedTaskContexts = jobContext.Queue.OrderBy(task => task.Priority);
            var queue = new ConcurrentQueue<IWorkerTaskContext>();
            foreach (var taskContext in orderedTaskContexts)
                queue.Enqueue(taskContext);
            return queue;
        }

        /// <summary>
        /// Gets a job context from storage by ID
        /// </summary>
        protected virtual async Task<IWorkerContext> GetJobContextAsync(string jobContextId,
            CancellationToken ct)
        {
            var jobContext = await _pipelineStorage.GetWorkerContextAsync(jobContextId, ct);
            return jobContext;
        }

        /// <summary>
        /// Get a new scope, for use with DI and inversion of control containers
        /// </summary>
        protected virtual IWorkerTaskFactoryScope GetJobDependencyScope()
        {
            return _taskFactory.GetScope();
        }

        /// <summary>
        /// Update the job context in the pipeline storage
        /// </summary>
        protected virtual async Task<IWorkerContext> UpdateJobContextAsync(
            IWorkerContext jobContext, CancellationToken ct)
        {
            await _pipelineStorage.UpdateJobContextAsync(jobContext, ct);
            return jobContext;
        }

        /// <summary>
        /// Update a task context within a job context in the pipeline storage
        /// </summary>
        protected virtual async Task<IWorkerContext> UpdateJobContextForTaskAsync(
            IWorkerContext jobContext, IWorkerTaskContext taskContext, CancellationToken ct)
        {
            var jobTaskContext = jobContext.Queue.SingleOrDefault(task => task.Id == taskContext.Id);
            jobTaskContext = taskContext;
            await _pipelineStorage.UpdateJobContextAsync(jobContext, ct);
            return jobContext;
        }

        #endregion

        #region Task context

        /// <summary>
        /// Created a task instance from the factory
        /// </summary>
        protected virtual IWorkerTask CreateTaskInstance(IWorkerTaskContext taskContext)
        {
            var task = _taskFactory.Create(taskContext.Task);
            return task;
        }

        /// <summary>
        /// Release a task instance by calling the factory
        /// </summary>
        protected virtual void ReleaseTaskInstance(IWorkerContext jobContext,
            IWorkerTaskContext taskContext, IWorkerTask taskInstance)
        {
            _taskFactory.Release(taskInstance);
        }

        /// <summary>
        /// Executes a task
        /// </summary>
        protected virtual Task<IWorkerTaskContext> ExecuteTaskAsync(IWorkerTask taskInstance,
            IWorkerTaskContext taskContext, IWorkerContext jobContext, CancellationToken ct)
        {
            var task = taskInstance.ExecuteTaskAsync(taskContext, jobContext, _pipelineStorage, ct);
            return task;
        }

        #endregion

        #region General

        /// <summary>
        /// Check for Hangfire or internal cancellation
        /// </summary>
        /// <param name="jct">A Hangfire cancellation token</param>
        /// <param name="cts">An internal cancellation token</param>
        protected virtual void CheckForCancellation(IJobCancellationToken jct,
            CancellationTokenSource cts)
        {
            cts.Token.ThrowIfCancellationRequested();
            try
            {
                jct.ThrowIfCancellationRequested();
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Hangfire job was cancelled");
                cts.Cancel();
                throw;
            }
        }

        #endregion

        #region Hooks

        /// <summary>
        /// A hook for after the task context has been updated
        /// </summary>
        protected virtual void OnJobContextUpdatedForTask(IWorkerContext jobContext,
            IWorkerTaskContext taskContext, IWorkerTask taskInstance)
        {
        }

        /// <summary>
        /// A hook for after task execution has completed
        /// </summary>
        protected virtual void OnTaskExecuted(IWorkerContext jobContext,
            IWorkerTaskContext taskContext, IWorkerTask taskInstance)
        {
        }

        /// <summary>
        /// A hook for after a task has started
        /// </summary>
        protected virtual void OnTaskStarted(IWorkerContext jobContext,
            IWorkerTaskContext taskContext)
        {
        }

        /// <summary>
        /// A hook for after the task instance has been created from the factory
        /// </summary>
        protected virtual void OnTaskInstanceCreated(IWorkerContext jobContext,
            IWorkerTaskContext taskContext, IWorkerTask taskInstance)
        {
        }

        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _pipelineStorage?.Dispose();
        }
    }
}
