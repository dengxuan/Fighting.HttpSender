using Baibaocp.Core.Messages;
using Baibaocp.LotteryNotifier.DependencyInjection;
using Fighting.Extensions.Messaging.DependencyInjection;
using Fighting.Extensions.Serialization.Json.DependencyInjection;
using Fighting.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
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
                            options.Configures.Add(new NoticeConfiguration { LvpVenderId = "100010", Url = "http://wcapi.baibaocp.com/ordernotify/index", SecurityKey = "1234qwer" });
                        });
                    });
                    services.AddScoped<IHostedService, LotteryNotifierServices>();
                });
            await host.RunConsoleAsync();
        }
    }
}
