using Fighting.Worker.Abstractions;
using Newtonsoft.Json.Converters;
using System;

namespace Fighting.Worker
{
    public class WorkerTaskContextJsonConverter : CustomCreationConverter<IWorkerTaskContext>
    {
        public override IWorkerTaskContext Create(Type objectType)
        {
            return new WorkerTaskContext();
        }
    }
}
