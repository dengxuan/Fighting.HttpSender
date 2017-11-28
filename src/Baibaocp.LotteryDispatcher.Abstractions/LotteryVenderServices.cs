using Fighting.Extensions.Messaging.Abstractions;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher
{
    public class LotteryVenderServices : BackgroundService
    {

        private readonly IRawMessageHandlerSubscriber _handlerSubscriber;

        public LotteryVenderServices(IRawMessageHandler messageHandler, IRawMessageHandlerSubscriber handlerSubscriber)
        {
            _handlerSubscriber = handlerSubscriber;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _handlerSubscriber.Subscribe(stoppingToken);
        }
    }
}
