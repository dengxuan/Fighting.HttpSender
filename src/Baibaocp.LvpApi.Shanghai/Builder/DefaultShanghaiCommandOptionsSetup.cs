using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryVender.Sending.Shanghai.Builder
{

    internal class DefaultShanghaiCommandOptionsSetup : ConfigureOptions<ShanghaiSenderOptions>
    {
        public DefaultShanghaiCommandOptionsSetup()
            : base(ConfigureOptions)
        {
        }

        private static void ConfigureOptions(ShanghaiSenderOptions options)
        {
            options.SecretKey = "ourpartner";
            options.Url = "http://115.29.193.120/";
            options.VenderId = "800";
        }
    }
}
