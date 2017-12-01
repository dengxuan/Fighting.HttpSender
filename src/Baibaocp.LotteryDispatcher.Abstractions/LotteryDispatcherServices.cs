using Fighting.Extensions.Messaging.Abstractions;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher
{
    public class LotteryDispatcherServices : BackgroundService
    {

        private readonly IRawMessageHandlerSubscriber _handlerSubscriber;

        public LotteryDispatcherServices(IRawMessageHandler messageHandler, IRawMessageHandlerSubscriber handlerSubscriber)
        {
            _handlerSubscriber = handlerSubscriber;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _handlerSubscriber.Subscribe(stoppingToken);
        }
    }
}
