using Fighting.Extensions.Messaging.DependencyInjection;
using Fighting.Extensions.Serialization.ProtoBuf.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Baibaocp.LotteryVender.DependencyInjection;
using Baibaocp.LotteryVender.Shanghai.DependencyInjection;
using System.Threading.Tasks;
using Baibaocp.LotteryVender.Messaging;
using Baibaocp.Core.Messages;

namespace Baibaocp.LotteryVender.Shanghai
{
    class Program
    {
        public static async Task Main(string[] args)
        {

            var host = new HostBuilder()
                //.UseServiceProviderFactory<MyContainer>(new MyContainerFactory())
                //.ConfigureContainer<MyContainer>((hostContext, container) =>
                //{
                //})
                //.ConfigureAppConfiguration((hostContext, config) =>
                //{
                //    config.AddEnvironmentVariables();
                //    config.AddJsonFile("appsettings.json", optional: true);
                //    config.AddCommandLine(args);
                //})
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddProtoBuf();
                    services.AddMessaging(messageBuilder =>
                    {
                        messageBuilder.AddRbbitMQ(options =>
                        {
                            options.VirtualHost = "/";
                            options.Servername = "192.168.1.21";
                            options.Username = "root";
                            options.Password = "123qwe";
                            options.ExchangeName = "Baibaocp.LotteryVender";
                        }).ConfigureOptions(messageOptions => 
                        {
                            // 订阅接单队列
                            messageOptions.AddConsumer<OrderingMessage>("Lvp.Orders.Dispatcher");
                        });
                    });
                    services.AddLotteryVender(builderAction =>
                    {
                        builderAction.AddHandlerDiscovery(discoverySettings =>
                        {
                            //discoverySettings.CommandHandlerAssemblies.Add(Assembly.Load(File.ReadAllBytes($"{AppContext.BaseDirectory}/Baibaocp.LotteryVender.Sending.Shanghai.dll")));
                            discoverySettings.IncludeNonPublic = true;
                        });
                        builderAction.AddShanghai(setupOptions =>
                        {
                            setupOptions.SecretKey = "ourpartner";
                            setupOptions.Url = "http://115.29.193.120/";
                            setupOptions.VenderId = "800";
                        });
                    });
                    services.AddScoped<IHostedService, LdpApiServices>();
                });

            //var s = host.Services;
            await host.RunConsoleAsync();

            //HostBuilder builder = new HostBuilder();
            //builder.ConfigureServices(services =>
            //{
            //    services.AddLvpApi(builderAction =>
            //    {
            //        builderAction.AddHandlerDiscovery(discoverySettings =>
            //        {
            //            //discoverySettings.CommandHandlerAssemblies.Add(Assembly.Load(File.ReadAllBytes($"{AppContext.BaseDirectory}/Baibaocp.LotteryVender.Sending.Shanghai.dll")));
            //            discoverySettings.IncludeNonPublic = true;
            //        });
            //        builderAction.AddShanghaiLvpApi(setupOptions =>
            //        {
            //            setupOptions.SecretKey = "ourpartner";
            //            setupOptions.Url = "http://115.29.193.120/";
            //            setupOptions.VenderId = "800";
            //        });
            //    });
            //});
            //IExecuterDispatcher sender = sp.GetRequiredService<IExecuterDispatcher>();

            //ExecuteResult executeResult = sender.DispatchAsync<OrderingExecuter>(new OrderingExecuter("100010")
            //{
            //    OrderId = Guid.NewGuid().ToString("N"),
            //    LotteryId = 23529,
            //    LotteryPlayId = 2,
            //    InvestAmount = 200,
            //    InvestCode = "08 12 15 19 28-02 07",
            //    InvestCount = 1,
            //    InvestTimes = 1,
            //    InvestType = false,
            //    IssueNumber = 2017135
            //}).GetAwaiter().GetResult();
            //if (executeResult.Success)
            //{
            //    Console.WriteLine();
            //}
        }
    }
}
