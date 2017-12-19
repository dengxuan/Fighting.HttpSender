using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Baibaocp.Hangfire.Hosting
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder().ConfigureServices((hostContext, services) =>
            {
                GlobalConfiguration.Configuration.UseRedisStorage("192.168.1.21:6379,password=zf8Mjjo6rLKDzf81,defaultDatabase=9");
                services.AddScoped<IHostedService, HangfireBackgroundService>();
            })
            .ConfigureAppConfiguration((hostContext, configure) =>
            {
                configure.AddEnvironmentVariables();
                configure.AddJsonFile("appsettings.json", optional: true);
                configure.AddCommandLine(args);
            });
            await host.RunConsoleAsync();
        }
    }
}
