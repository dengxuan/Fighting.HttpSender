using Baibaocp.LvpApi;
using Baibaocp.LvpApi.Abstractions;
using Baibaocp.LvpApi.DependencyInjection;
using Baibaocp.LotteryVender.Sending.Shanghai;
using Baibaocp.LotteryVender.Sending.Shanghai.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Baibaocp.LvpApi.Executers;

namespace Baibaocp.LotteryVender.Sending.Order
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceCollection services = new ServiceCollection();
            services.AddExecuter(builderAction =>
            {
                builderAction.AddHandlerDiscovery(discoverySettings =>
                {
                    //discoverySettings.CommandHandlerAssemblies.Add(Assembly.Load(File.ReadAllBytes($"{AppContext.BaseDirectory}/Baibaocp.LotteryVender.Sending.Shanghai.dll")));
                    discoverySettings.IncludeNonPublic = true;
                });
                builderAction.AddShanghaiLvpApi(setupOptions =>
                {
                    setupOptions.SecretKey = "ourpartner";
                    setupOptions.Url = "http://115.29.193.120/";
                    setupOptions.VenderId = "800";
                });
            });
            services.AddOptions();
            IServiceProvider sp = services.BuildServiceProvider();
            IExecuterDispatcher sender = sp.GetRequiredService<IExecuterDispatcher>();

            ExecuteResult executeResult = sender.DispatchAsync<OrderingExecuter>(new OrderingExecuter("100010")
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
