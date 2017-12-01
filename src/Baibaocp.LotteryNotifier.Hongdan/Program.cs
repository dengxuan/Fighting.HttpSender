﻿using Baibaocp.Core.Messages;
using Baibaocp.LotteryNotifier.DependencyInjection;
using Fighting.Extensions.Messaging.DependencyInjection;
using Fighting.Extensions.Serialization.Json.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Hongdan
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
                            discoverySettings.MessageHandlerAssemblies.Add(typeof(LotteryNotifierServices).Assembly);
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
                            messageOptions.AddConsumer<TicketingMessage>("Lvp.Tickets.Notifier");
                            messageOptions.AddConsumer<AwardingMessage>("Lvp.Awards.Notifier");
                        });
                    });
                    services.AddLotteryNotifier(builderAction =>
                    {
                        builderAction.ConfigureOptions(options =>
                        {
                            options.Configures.Add(new NoticeConfiguration { LvpVenderId = "10010", Url = "http://127.0.0.1:6000", SecurityKey = "10010" });
                        });
                    });
                    services.AddScoped<IHostedService, LotteryNotifierServices>();
                });
            await host.RunConsoleAsync();
        }
    }
}
