using System.Threading;
using System.Threading.Tasks;
using Baibaocp.Core.Messages;
using Fighting.Extensions.Messaging.Abstractions;
using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.Notifiers;

namespace Baibaocp.LotteryNotifier.Internal.MessageHandlers
{
    public class TicketedMessageHandler : IMessageHandler<TicketingMessage>
    {
        private readonly INoticeDispatcher _dispatcher;

        public TicketedMessageHandler(INoticeDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task<bool> Handle(TicketingMessage message, CancellationToken token)
        {
            bool result = await _dispatcher.DispatchAsync(new TicketedNotifier
            {
                OrderId = message.LvpOrderId,
                LvpVenderId = message.LvpVenderId,
                Status = message.TicketStatus
            });
            return result;
        }
    }
}
