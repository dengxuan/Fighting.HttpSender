using Baibaocp.LotteryVender.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Baibaocp.LotteryVender.Shanghai.DependencyInjection
{
    public static class ShanghaiExecuterBuilderExtensions
    {
        public static LvpApiBuilder AddShanghai(this LvpApiBuilder builder, Action<ShanghaiSenderOptions> setupOptions)
        {
            builder.Services.AddSingleton(c => c.GetRequiredService<IOptions<ShanghaiSenderOptions>>().Value);
            builder.Services.Configure(setupOptions);
            return builder;
        }
    }
}
