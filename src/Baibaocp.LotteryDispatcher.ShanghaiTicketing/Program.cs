using Baibaocp.Core.Messages;
using Baibaocp.LotteryDispatcher.DependencyInjection;
using Baibaocp.LotteryDispatcher.Shanghai.DependencyInjection;
using Fighting.Extensions.Messaging.DependencyInjection;
using Fighting.Extensions.Serialization.Json.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.ShanghaiTicketing
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddJson();
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
                            // 订阅投注队列
                            messageOptions.AddConsumer<OrderingMessage>("Ldp.Tickets.Dispatcher");
                        });
                    });
                    services.AddLotteryVender(builderAction =>
                    {
                        builderAction.AddShanghai(setupOptions =>
                        {
                            setupOptions.SecretKey = "ourpartner";
                            setupOptions.Url = "http://115.29.193.120/";
                            setupOptions.VenderId = "800";
                        });
                    });
                    services.AddScoped<IHostedService, LotteryVenderServices>();
                });

            await host.RunConsoleAsync();
        }
    }
}
