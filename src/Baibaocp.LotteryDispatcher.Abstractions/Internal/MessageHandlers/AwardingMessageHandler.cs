using Baibaocp.Core.Messages;
using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.Executers;
using Fighting.Extensions.Messaging.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.ShanghaiAwarding.Handlers
{
    public class AwardingMessageHandler : IMessageHandler<AwardingMessage>
    {

        private readonly IMessagePublisher _publisher;

        private readonly IExecuterDispatcher<AwardingExecuter> _awardingDispatcher;

        public AwardingMessageHandler(IMessagePublisher publisher, IExecuterDispatcher<AwardingExecuter> awardingDispatcher)
        {
            _publisher = publisher;
            _awardingDispatcher = awardingDispatcher;
        }

        public async Task<bool> Handle(AwardingMessage message, CancellationToken token)
        {
            if (!token.IsCancellationRequested)
            {
                AwardingExecuter executer = new AwardingExecuter(message.LdpVenderId, message.LdpOrderId, message.LvpOrders);
                var result = await _awardingDispatcher.DispatchAsync(executer);
                return result;
            }
            return false;
        }
    }
}
