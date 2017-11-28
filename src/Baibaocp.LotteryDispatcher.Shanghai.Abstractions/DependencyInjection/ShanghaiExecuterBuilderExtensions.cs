using Baibaocp.LotteryDispatcher.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Baibaocp.LotteryDispatcher.Shanghai.DependencyInjection
{
    public static class ShanghaiExecuterBuilderExtensions
    {
        public static LotteryDispatcherBuilder AddShanghai(this LotteryDispatcherBuilder builder, Action<ShanghaiDispatcherOptions> setupOptions)
        {
            builder.Services.AddSingleton(c => c.GetRequiredService<IOptions<ShanghaiDispatcherOptions>>().Value);
            builder.Services.Configure(setupOptions);
            return builder;
        }
    }
}
