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
                    services.AddMessaging(messageBuilder =>
                    {
                        messageBuilder.AddHandlerDiscovery(discoverySettings =>
                        {
                            discoverySettings.MessageHandlerAssemblies.Add(typeof(LotteryVenderServices).Assembly);
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
                            setupOptions.VenderId = "800";
                        });
                        builderAction.ConfigureOptions(options =>
                        {
                            options.AddHandler<ShanghaiOrderingExecuteHandler, OrderingExecuter, OrderingResult>("800");
                            options.AddHandler<ShanghaiOrderingExecuteHandler, OrderingExecuter, OrderingResult>("900");
                            options.AddHandler<ShanghaiAwardingExecuteHandler, AwardingExecuter, AwardingResult>("800");
                            options.AddHandler<ShanghaiTicketingExecuteHandler, TicketingExecuter, TicketingResult>("800");
                        });
                    });
                    services.AddScoped<IHostedService, LotteryVenderServices>();
                });

            await host.RunConsoleAsync();
        }
    }
}
