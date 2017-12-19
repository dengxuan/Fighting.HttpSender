using Fighting.Worker.Abstractions;
using Hangfire;
using System;

namespace Fighting.Worker
{
    /// <summary>
    /// Overrides the Hangfire JobActivator to always use the pipeline server
    /// </summary>
    public class WorkerActivator : JobActivator
    {
        private readonly IWorkerServer _pipelineServer;

        public WorkerActivator(IWorkerServer pipelineServer)
        {
            _pipelineServer = pipelineServer;
        }

        public override object ActivateJob(Type jobType)
        {
            return _pipelineServer;
        }

        [Obsolete]
        public override JobActivatorScope BeginScope()
        {
            return new PipelineJobActivatorScope(_pipelineServer);
        }

        private class PipelineJobActivatorScope : JobActivatorScope
        {
            private readonly IWorkerServer _pipelineServer;

            public PipelineJobActivatorScope(IWorkerServer pipelineServer)
            {
                _pipelineServer = pipelineServer;
            }

            public override object Resolve(Type type)
            {
                return _pipelineServer;
            }
        }
    }
}
