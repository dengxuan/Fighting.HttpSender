using Baibaocp.LotteryDispatcher.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Baibaocp.LotteryDispatcher.DependencyInjection
{
    public static class LotteryVenderIServiceCollectionExtensions
    {
        public static IServiceCollection AddLotteryDispatcher(this IServiceCollection services, Action<LotteryDispatcherBuilder> builderAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (builderAction == null)
            {
                throw new ArgumentNullException(nameof(builderAction));
            }

            var builder = new LotteryDispatcherBuilder(services);
            builderAction.Invoke(builder);
            builder.Build();

            return services;
        }
    }
}
