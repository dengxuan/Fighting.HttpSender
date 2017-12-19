using Fighting.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fighting.Worker.DependencyInjection
{
    public static class WorkerIServiceCollectionExtensions
    {
        public static void AddWorker(this IServiceCollection services, Action<WorkBuilder> buildAction)
        {
            WorkBuilder builder = new WorkBuilder(services);
            builder.Build();
        }
    }
}
