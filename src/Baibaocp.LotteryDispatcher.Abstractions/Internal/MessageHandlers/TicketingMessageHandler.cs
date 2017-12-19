using Baibaocp.Core.Messages;
using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.Executers;
using Fighting.Extensions.Messaging.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.ShanghaiTicketing.Handlers
{
    public class TicketingMessageHandler : IMessageHandler<TicketingMessage>
    {

        private readonly IMessagePublisher _publisher;

        private readonly IExecuterDispatcher<TicketingExecuter> _dispatcher;

        public TicketingMessageHandler(IMessagePublisher publisher, IExecuterDispatcher<TicketingExecuter> dispatcher)
        {
            _publisher = publisher;
            _dispatcher = dispatcher;
        }

        public async Task<bool> Handle(TicketingMessage message, CancellationToken token)
        {
            if (!token.IsCancellationRequested)
            {
                TicketingExecuter executer = new TicketingExecuter(message.LdpVenderId, message.LdpOrderId, message.LvpOrders);
                var result = await _dispatcher.DispatchAsync(executer);
                return result;
            }
            return true;
        }
    }
}
