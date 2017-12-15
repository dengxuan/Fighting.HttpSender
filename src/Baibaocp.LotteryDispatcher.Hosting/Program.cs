using Baibaocp.Core.Messages;
using Fighting.Extensions.Messaging.DependencyInjection;
using Fighting.Extensions.Serialization.Json.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Baibaocp.LotteryDispatcher.DependencyInjection;
using System;
using System.Threading.Tasks;
using Baibaocp.LotteryDispatcher.Shanghai.Handlers;
using Baibaocp.LotteryDispatcher.Shanghai.DependencyInjection;
using Baibaocp.LotteryDispatcher.Executers;
using Baibaocp.LotteryDispatcher.Models.Results;
using Microsoft.Extensions.Logging;
using Fighting.Extensions.Caching.DependencyInjection;
using Fighting.Extensions.Caching.Redis.DependencyInjection;
using Fighting.Storaging.DependencyInjection;

namespace Baibaocp.LotteryDispatcher.Hosting
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
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
                            discoverySettings.MessageHandlerAssemblies.Add(typeof(LotteryDispatcherServices).Assembly);
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
                            messageOptions.AddConsumer<OrderingMessage>("Ldp.Orders.Dispatcher");
                            messageOptions.AddConsumer<AwardingMessage>("Ldp.Awards.Dispatcher");
                            messageOptions.AddConsumer<TicketingMessage>("Ldp.Tickets.Dispatcher");
                        });
                    });
                    services.AddLotteryDispatcher(builderAction =>
                    {
                        builderAction.AddShanghai(setupOptions =>
                        {
                            setupOptions.SecretKey = "ourpartner";
                            setupOptions.Url = "http://115.29.193.120/";
                        });
                        builderAction.ConfigureOptions(options =>
                        {
                            options.AddHandler<ShanghaiOrderingExecuteHandler, OrderingExecuter, OrderingResult>("800");
                            options.AddHandler<ShanghaiOrderingExecuteHandler, OrderingExecuter, OrderingResult>("900");
                            options.AddHandler<ShanghaiAwardingExecuteHandler, AwardingExecuter, AwardingResult>("800");
                            options.AddHandler<ShanghaiTicketingExecuteHandler, TicketingExecuter, TicketingResult>("800");
                        });
                    });
                    services.AddStoraging(storageBuilder => 
                    {
                        storageBuilder.ConfigureOptions(options =>
                        {
                            options.DefaultNameOrConnectionString = "server=192.168.1.21; database=Baibaocp; uid=dba; password=L4H]JtuA2RaWl@^]S$9a4dN-!,01Z7Qs;";
                        });
                    });
                    services.AddScoped<IHostedService, LotteryDispatcherServices>();
                });
            host.ConfigureLogging(configureLogging => configureLogging.ClearProviders().AddConsole().AddDebug().SetMinimumLevel(LogLevel.Critical));
            await host.RunConsoleAsync();
        }
    }
}
