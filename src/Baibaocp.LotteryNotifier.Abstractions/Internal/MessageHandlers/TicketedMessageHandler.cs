using System.Threading;
using System.Threading.Tasks;
using Baibaocp.Core.Messages;
using Fighting.Extensions.Messaging.Abstractions;
using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.Notifiers;

namespace Baibaocp.LotteryNotifier.Internal.MessageHandlers
{
    public class TicketedMessageHandler : IMessageHandler<TicketedMessage>
    {
        private readonly INoticeDispatcher _dispatcher;

        public TicketedMessageHandler(INoticeDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task<bool> Handle(TicketedMessage message, CancellationToken token)
        {
            bool result = await _dispatcher.DispatchAsync(new Notifier<Ticketed>(message.LvpVenderId)
            {
                Notice = new Ticketed
                {
                    OrderId = message.LvpOrderId,
                    TicketOdds = message.TicketOdds,
                    Status = message.Status == Core.OrderStatus.TicketDrawing ? 10300 : 10301
                }
            });
            return result;
        }
    }
}
