using Microsoft.Extensions.DependencyInjection;
using System;

namespace Fighting.Worker.Builder
{
    public class WorkBuilder
    {
        public IServiceCollection Services { get; }

        internal WorkBuilder(IServiceCollection services) => Services = services;

        public WorkBuilder ConfigureOptions(Action<WorkerStorageOptions> options)
        {
            Services.Configure(options);
            return this;
        }

        public void Build()
        {

        }
    }
}
