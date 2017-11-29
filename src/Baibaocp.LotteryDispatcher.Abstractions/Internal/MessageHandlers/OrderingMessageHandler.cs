using Baibaocp.Core;
using Baibaocp.Core.Messages;
using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.Executers;
using Baibaocp.LotteryDispatcher.Models.Results;
using Fighting.Extensions.Messaging.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Internal.MessageHandlers
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
                OrderingExecuter executer = new OrderingExecuter(message.LvpVenderId, message.LdpVenderId)
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
                var executeResult = await _dispatcher.DispatchAsync<OrderingExecuter, OrderingResult>(executer);
                if (executeResult.Success)
                {
                    TicketingMessage ticketingMessage = new TicketingMessage
                    {
                        OrderId = message.OrderId,
                        LvpOrderId = message.LvpOrderId,
                        LvpVenderId = message.LvpVenderId,
                        LdpVenderId = executeResult.VenderId,
                        TicketStatus = executeResult.Result.Code
                    };
                    if (executeResult.Result.Code == OrderStatus.Ordering.Success)
                    {
                        await _publisher.Publish(RoutingkeyConsts.Orders.Completed.Success, ticketingMessage, token);
                        return true;
                    }
                    else if (executeResult.Result.Code == OrderStatus.Ordering.Failure)
                    {
                        await _publisher.Publish(RoutingkeyConsts.Orders.Completed.Failure, ticketingMessage, token);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
