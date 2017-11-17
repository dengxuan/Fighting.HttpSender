using Baibaocp.LotteryVender.Sending.Shanghai.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryVender.Sending.Shanghai
{
    public class Setup
    {
        public void Init(IServiceCollection services)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<ShanghaiSenderOptions>, DefaultShanghaiCommandOptionsSetup>());
            services.AddSingleton(c => c.GetRequiredService<IOptions<ShanghaiSenderOptions>>().Value);
        }
    }
}
