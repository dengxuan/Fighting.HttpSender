using Baibaocp.LotteryVender.Abstractions;
using Baibaocp.LotteryVender.Executers;
using Fighting.Extensions.Messaging.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryVender.Messaging
{
    public class OrderingMessageHandler : IMessageHandler<OrderingMessage>
    {

        private readonly IMessagePublisher _publisher;

        private readonly IExecuterDispatcher _dispatcher;

        public OrderingMessageHandler(IMessagePublisher publisher, IExecuterDispatcher dispatcher)
        {
            _publisher = publisher;
            _dispatcher = dispatcher;
        }

        public async Task<bool> Handle(OrderingMessage message, CancellationToken token)
        {
            if (!token.IsCancellationRequested)
            {
                OrderingExecuter executer = new OrderingExecuter()
                {
                    OrderId = message.OrderId,
                    InvestAmount = message.InvestAmount,
                    InvestCode = message.InvestCode,
                    InvestCount = message.InvestCount,
                    InvestTimes = message.InvestTimes,
                    InvestType = message.InvestType,
                    IssueNumber = message.IssueNumber,
                    LotteryId = message.LotteryId,
                    LotteryPlayId = message.LotteryPlayId
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
                await _publisher.Publish(executeResult.Success ? "Tickets.Completed.Success" : "Tickets.Completed.Failure", message, token);
                return executeResult.Success;
            }
            return false;
        }
    }
}
