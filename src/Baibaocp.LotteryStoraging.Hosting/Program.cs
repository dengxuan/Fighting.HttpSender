using Baibaocp.Core.Messages;
using Baibaocp.LotteryStoraging.Abstractions;
using Baibaocp.Storaging.EntityFrameworkCore;
using Fighting.Extensions.Caching.DependencyInjection;
using Fighting.Extensions.Caching.Redis.DependencyInjection;
using Fighting.Extensions.Messaging.DependencyInjection;
using Fighting.Extensions.Serialization.Json.DependencyInjection;
using Fighting.Storaging.EntityFrameworkCore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Baibaocp.LotteryStoraging.Hosting
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddJson();
                    services.AddCaching(cacheBuilder =>
                    {
                        cacheBuilder.AddRedis(options =>
                        {
                            options.DatabaseId = 10;
                            options.ConnectionString = "192.168.1.21:6379,password=zf8Mjjo6rLKDzf81";
                        });
                    });
                    services.AddMessaging(messageBuilder =>
                    {
                        messageBuilder.AddHandlerDiscovery(discoverySettings =>
                        {
                            discoverySettings.MessageHandlerAssemblies.Add(typeof(LotteryStoragingServices).Assembly);
                        });
                        messageBuilder.AddRbbitMQ(options =>
                        {
                            options.VirtualHost = "/";
                            options.Servername = "192.168.1.21";
                            options.Username = "root";
                            options.Password = "123qwe";
                            options.ExchangeName = "Baibaocp.LotteryVender";
                        }).ConfigureOptions(messageOptions =>
                        {
                            messageOptions.AddConsumer<OrderingMessage>("Orders.Storaging");
                            messageOptions.AddConsumer<AwardingMessage>("Awards.Storaging");
                            messageOptions.AddConsumer<TicketingMessage>("Tickets.Storaging");
                        });
                    });


                    services.AddEntityFrmaeworkStorage<BaibaocpStorage>(storageOptions =>
                    {
                        storageOptions.DefaultNameOrConnectionString = "server=192.168.1.21; database=Baibaocp; uid=dba; password=L4H]JtuA2RaWl@^]S$9a4dN-!,01Z7Qs;";
                    });

                    services.AddScoped<IHostedService, LotteryStoragingServices>();
                });
            await host.RunConsoleAsync();
        }
    }
}
