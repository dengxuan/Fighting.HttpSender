using Baibaocp.Core.Messages;
using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.Notifiers;
using Fighting.Extensions.Messaging.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Internal.MessageHandlers
{
    public class AwardedMessageHandler : IMessageHandler<AwardingMessage>
    {
        private readonly INoticeDispatcher _dispatcher;

        public AwardedMessageHandler(INoticeDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task<bool> Handle(AwardingMessage message, CancellationToken token)
        {
            bool result = await _dispatcher.DispatchAsync(new Notifier<Awarded>(message.LdpVenderId)
            {
                Notice = new Awarded
                {
                    OrderId = message.OrderId,
                    Status = message.Status == Core.OrderStatus.TicketWinning ? 10400 : 10401,
                    Amount = message.Amount
                }
            });
            return result;
        }
    }
}
