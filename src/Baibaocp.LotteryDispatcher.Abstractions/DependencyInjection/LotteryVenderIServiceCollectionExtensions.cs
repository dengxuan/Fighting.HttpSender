using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Baibaocp.LotteryDispatcher.DependencyInjection
{
    public static class LotteryVenderIServiceCollectionExtensions
    {
        public static IServiceCollection AddLotteryVender(this IServiceCollection services, Action<LotteryDispatcherBuilder> builderAction)
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

            var builder = new LotteryDispatcherBuilder(services);
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
