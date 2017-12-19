using Fighting.Worker.Abstractions;
using System;
using System.Collections.Generic;

namespace Fighting.Worker
{
    public class WorkerContext : IWorkerContext
    {
        public virtual string Id { get; set; }
        public virtual string HangfireId { get; set; }
        public virtual string HangfireQueue { get; set; }
        public virtual IEnumerable<IWorkerTaskContext> Queue { get; set; }
        public virtual object State { get; set; }
        public virtual IDictionary<string, object> Environment { get; set; }
        public virtual IDictionary<string, object> Result { get; set; }
        public virtual IEnumerable<IWorkerTaskContext> Completed { get; set; }
        public virtual DateTime Start { get; set; }
        public virtual DateTime End { get; set; }
    }
}