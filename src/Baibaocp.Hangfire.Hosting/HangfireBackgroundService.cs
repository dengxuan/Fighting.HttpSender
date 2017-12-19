using Hangfire;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.Hangfire.Hosting
{
    public class HangfireBackgroundService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                using (BackgroundJobServer server = new BackgroundJobServer())
                {
                    stoppingToken.WaitHandle.WaitOne(Timeout.Infinite);
                    server.SendStop();
                }
            });
        }
    }
}
