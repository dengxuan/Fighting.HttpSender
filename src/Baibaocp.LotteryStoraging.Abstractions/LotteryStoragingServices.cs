using Fighting.Extensions.Messaging.Abstractions;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryStoraging.Abstractions
{
    public class LotteryStoragingServices : BackgroundService
    {
        private readonly IRawMessageHandlerSubscriber _handlerSubscriber;

        public LotteryStoragingServices(IRawMessageHandler messageHandler, IRawMessageHandlerSubscriber handlerSubscriber)
        {
            _handlerSubscriber = handlerSubscriber;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _handlerSubscriber.Subscribe(stoppingToken);
        }
    }
}
