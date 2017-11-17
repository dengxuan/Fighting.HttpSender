using Baibaocp.LotteryCommand;
using Baibaocp.LotteryCommand.Abstractions;
using Baibaocp.LotteryCommand.DependencyInjection;
using Baibaocp.LotteryVender.Sending.Shanghai;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace Baibaocp.LotteryVender.Sending.Order
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage res = client.GetAsync("http://www.baidu.com").GetAwaiter().GetResult();
            ServiceCollection services = new ServiceCollection();
            services.AddLotteryCommand(builderAction =>
            {
                builderAction.AddHandlerDiscovery(discoverySettings =>
                {
                    discoverySettings.CommandHandlerAssemblies.Add(Assembly.Load(File.ReadAllBytes($"{AppContext.BaseDirectory}/Baibaocp.LotteryVender.Sending.Shanghai.dll")));
                    discoverySettings.IncludeNonPublic = true;
                });
                //builderAction.AddShanghaiCommand(setupOptions =>
                //{
                //    setupOptions.SecretKey = "ourpartner";
                //    setupOptions.Url = "http://115.29.193.120/";
                //    setupOptions.VenderId = "800";
                //});
            });
            //services.AddSingleton<ShanghaiSenderOptions>(new ShanghaiSenderOptions
            //{
            //    CommandId = "101",
            //    SecretKey = "ourpartner",
            //    Url = "http://115.29.193.120/",
            //    VenderId = "800"
            //});
            services.AddOptions();
            IServiceProvider sp = services.BuildServiceProvider();
            ICommandSender sender = sp.GetRequiredService<ICommandSender>();

            ExecuteResult executeResult = sender.SendAsync<OrderingCommand, (string orderId, bool succes)>(new OrderingCommand
            {
                OrderId = Guid.NewGuid().ToString("N"),
                LotteryId = 23529,
                LotteryPlayId = 2,
                InvestAmount = 200,
                InvestCode = "08 12 15 19 28-02 07",
                InvestCount = 1,
                InvestTimes = 1,
                InvestType = false,
                IssueNumber = 2017135
            }).GetAwaiter().GetResult();
            if (executeResult.Success)
            {
                Console.WriteLine();
            }
        }
    }
}
