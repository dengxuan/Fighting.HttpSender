using Baibaocp.LvpApi.Abstractions;
using Baibaocp.LvpApi.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LvpApi.DependencyInjection
{
    public static class LvpApiIServiceCollectionExtensions
    {
        public static IServiceCollection AddExecuter(this IServiceCollection services, Action<LvpApiBuilder> builderAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (builderAction == null)
            {
                throw new ArgumentNullException(nameof(builderAction));
            }

            AddLvpApiServices(services);

            var builder = new LvpApiBuilder(services);
            builderAction.Invoke(builder);
            builder.Build();

            return services;
        }

        internal static void AddLvpApiServices(IServiceCollection services)
        {
            services.AddSingleton<IExecuterDispatcher, ExecuterDispatcher>();
        }
    }
}
