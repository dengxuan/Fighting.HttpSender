using System;
using System.Threading.Tasks;

namespace Fighting.Worker.Abstractions
{
    /// <summary>
    /// A wrapper over the Hangfire client that works with pipeline jobs
    /// </summary>
    public interface IWorkerClient : IDisposable
    {
        /// <summary>
        /// Accessor for pipeline storage
        /// </summary>
        IWorkerStorage Storage { get; }

        /// <summary>
        /// Delete a job context from Hangfire so it will no longer be executed
        /// </summary>
        Task<IWorkerContext> DeleteAsync(IWorkerContext jobContext);

        /// <summary>
        /// Enqueue a job context with Hangfire to execute it as soon as possible
        /// </summary>
        Task<IWorkerContext> EnqueueAsync(IWorkerContext jobContext);

        /// <summary>
        /// Requeue a previously created Hangfire job
        /// </summary>
        Task<IWorkerContext> RequeueAsync(IWorkerContext jobContext);

        /// <summary>
        /// Schedule a job to run at some point in future
        /// </summary>
        Task<IWorkerContext> ScheduleAsync(IWorkerContext jobContext, TimeSpan delay);

        /// <summary>
        /// Schedule a job to run at some point in future
        /// </summary>
        Task<IWorkerContext> ScheduleAsync(IWorkerContext jobContext, DateTimeOffset delay);
    }
}
