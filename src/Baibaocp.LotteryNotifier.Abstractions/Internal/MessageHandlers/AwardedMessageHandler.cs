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
            bool result = await _dispatcher.DispatchAsync(new AwardedNotifier
            {
                LvpVenderId = message.LvpVenderId,
                OrderId = message.LvpOrderId,
                Amount = message.Amount
            });
            return result;
        }
    }
}
