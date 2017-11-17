using Baibaocp.LotteryCommand.Abstractions;
using Baibaocp.LotteryCommand.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryCommand.DependencyInjection
{
    public static class LotteryCommandIServiceCollectionExtensions
    {
        public static IServiceCollection AddLotteryCommand(this IServiceCollection services, Action<LotteryCommandBuilder> builderAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (builderAction == null)
            {
                throw new ArgumentNullException(nameof(builderAction));
            }

            AddLotteryCommandServices(services);

            var builder = new LotteryCommandBuilder(services);
            builderAction.Invoke(builder);
            builder.Build();

            return services;
        }

        internal static void AddLotteryCommandServices(IServiceCollection services)
        {
            services.AddSingleton<ICommandSender, CommandSender>();
        }
    }
}
