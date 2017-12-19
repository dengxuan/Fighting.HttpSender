using Fighting.Worker.Abstractions;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
namespace Fighting.Worker
{
    /// <summary>
    /// Default pipeline client wrapper over the Hangfire client for creating pipeline jobs
    /// </summary>
    public class WorkerClient : IWorkerClient
    {
        public readonly ILogger<WorkerClient> _logger;

        private readonly IBackgroundJobClient _hangfireClient;

        public WorkerClient(IWorkerStorage storage, IBackgroundJobClient hangfireClient, ILogger<WorkerClient> logger)
        {
            Storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _hangfireClient = hangfireClient ?? throw new ArgumentNullException(nameof(hangfireClient));
            _logger = logger;
        }

        public virtual IWorkerStorage Storage { get; }

        public virtual Task<IWorkerContext> DeleteAsync(IWorkerContext jobContext)
        {
            if (string.IsNullOrEmpty(jobContext.HangfireId))
                throw new ArgumentNullException(nameof(jobContext.HangfireId));
            if (_hangfireClient.Delete(jobContext.HangfireId))
            {
                _logger.LogInformation("Deleted Hangfire ID '{0}'", jobContext.HangfireId);
                jobContext.HangfireId = null;
            }
            else
            {
                _logger.LogWarning("Failed to delete Hangfire ID '{0}'", jobContext.HangfireId);
            }
            return Task.FromResult(jobContext);
        }

        public virtual Task<IWorkerContext> EnqueueAsync(IWorkerContext jobContext)
        {
            if (string.IsNullOrEmpty(jobContext.Id))
                throw new ArgumentNullException(nameof(jobContext.Id));
            jobContext.HangfireId = _hangfireClient.Enqueue<IWorkerServer>(server => server.ExecuteJob(jobContext.Id, JobCancellationToken.Null));
            _logger.LogInformation("Enqueued ID '{0}' on Hangfire ID '{1}'", jobContext.Id, jobContext.HangfireId);
            return Task.FromResult(jobContext);
        }

        public virtual Task<IWorkerContext> RequeueAsync(IWorkerContext jobContext)
        {
            if (string.IsNullOrEmpty(jobContext.HangfireId))
                throw new ArgumentNullException(nameof(jobContext.HangfireId));
            if (_hangfireClient.Requeue(jobContext.HangfireId))
            {
                _logger.LogInformation("Requeued Hangfire ID '{0}'", jobContext.HangfireId);
            }
            else
            {
                _logger.LogWarning("Failed to requeue Hangfire ID '{0}'", jobContext.HangfireId);
            }
            return Task.FromResult(jobContext);
        }

        /// <summary>
        /// Schedules a pipeline job in Hangfire
        /// </summary>
        public virtual Task<IWorkerContext> ScheduleAsync(IWorkerContext jobContext, DateTimeOffset enqueueAt)
        {
            if (string.IsNullOrEmpty(jobContext.Id))
                throw new ArgumentNullException(nameof(jobContext.Id));
            jobContext.HangfireId = _hangfireClient.Schedule<IWorkerServer>(server => server.ExecuteJob(jobContext.Id, JobCancellationToken.Null), enqueueAt);
            _logger.LogInformation("Scheduled ID '{0}' on Hangfire ID '{0}'", jobContext.Id, jobContext.HangfireId);
            return Task.FromResult(jobContext);
        }

        public virtual Task<IWorkerContext> ScheduleAsync(IWorkerContext jobContext,
            TimeSpan delay)
        {
            if (string.IsNullOrEmpty(jobContext.Id))
                throw new ArgumentNullException(nameof(jobContext.Id));
            jobContext.HangfireId = _hangfireClient.Schedule<IWorkerServer>(server => server.ExecuteJob(jobContext.Id, JobCancellationToken.Null), delay);
            _logger.LogInformation("Scheduled ID '{0}' on Hangfire ID '{0}'", jobContext.Id, jobContext.HangfireId);
            return Task.FromResult(jobContext);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
