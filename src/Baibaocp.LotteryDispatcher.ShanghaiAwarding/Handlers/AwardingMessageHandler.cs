using Baibaocp.Core.Messages;
using Baibaocp.LotteryDispatcher.Abstractions;
using Fighting.Extensions.Messaging.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.ShanghaiAwarding.Handlers
{
    public class AwardingMessageHandler : IMessageHandler<OrderingMessage>
    {

        private readonly IMessagePublisher _publisher;

        private readonly IExecuterDispatcher _dispatcher;

        public AwardingMessageHandler(IMessagePublisher publisher, IExecuterDispatcher dispatcher)
        {
            _publisher = publisher;
            _dispatcher = dispatcher;
        }

        public async Task<bool> Handle(OrderingMessage message, CancellationToken token)
        {
            if (!token.IsCancellationRequested)
            {
                AwardingExecuter executer = new AwardingExecuter(message.LvpVenderId, message.LdpVenderId)
                {
                    OrderId = message.OrderId,
                };
                var executeResult = await _dispatcher.DispatchAsync(executer);
                if (executeResult.Success)
                {
                    message.LdpVenderId = executeResult.VenderId;
                    message.Status = 1;
                }
                else
                {
                    message.Status = 0;
                }
                await _publisher.Publish(executeResult.Success ? "Awards.Completed.Success" : "Tickets.Completed.Failure", message, token);
                return true;
            }
            return false;
        }
    }
}
