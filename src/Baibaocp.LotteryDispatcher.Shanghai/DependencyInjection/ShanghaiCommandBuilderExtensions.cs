using Baibaocp.LotteryCommand.Builder;
using Baibaocp.LotteryVender.Sending.Shanghai.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryVender.Sending.Shanghai.DependencyInjection
{
    public static class ShanghaiCommandBuilderExtensions
    {
        public static LotteryCommandBuilder AddShanghaiCommand(this LotteryCommandBuilder builder, Action<ShanghaiSenderOptions> setupOptions)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<ShanghaiSenderOptions>, DefaultShanghaiCommandOptionsSetup>());
            builder.Services.AddSingleton(c => c.GetRequiredService<IOptions<ShanghaiSenderOptions>>().Value);
            //builder.Services.Configure(setupOptions);
            return builder;
        }
    }
}
