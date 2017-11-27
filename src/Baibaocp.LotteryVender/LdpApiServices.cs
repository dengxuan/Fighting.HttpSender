using Fighting.Extensions.Messaging.Abstractions;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryVender
{
    public class LdpApiServices : BackgroundService
    {

        private readonly IRawMessageHandlerSubscriber _handlerSubscriber;

        public LdpApiServices(IRawMessageHandler messageHandler, IRawMessageHandlerSubscriber handlerSubscriber)
        {
            _handlerSubscriber = handlerSubscriber;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _handlerSubscriber.Subscribe(stoppingToken);
        }
    }
}
