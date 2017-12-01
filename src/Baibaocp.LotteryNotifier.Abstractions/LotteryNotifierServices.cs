using Fighting.Extensions.Messaging.Abstractions;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier
{
    public class LotteryNotifierServices : BackgroundService
    {

        private readonly IRawMessageHandlerSubscriber _handlerSubscriber;

        public LotteryNotifierServices(IRawMessageHandler messageHandler, IRawMessageHandlerSubscriber handlerSubscriber)
        {
            _handlerSubscriber = handlerSubscriber;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _handlerSubscriber.Subscribe(stoppingToken);
        }
    }
}