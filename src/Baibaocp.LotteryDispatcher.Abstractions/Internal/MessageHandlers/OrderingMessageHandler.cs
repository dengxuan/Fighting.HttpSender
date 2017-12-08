using Baibaocp.Core;
using Baibaocp.Core.Messages;
using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.Executers;
using Baibaocp.LotteryDispatcher.Models.Results;
using Fighting.Extensions.Caching.Abstractions;
using Fighting.Extensions.Messaging.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Internal.MessageHandlers
{
    public class OrderingMessageHandler : IMessageHandler<OrderingMessage>
    {

        private readonly IMessagePublisher _publisher;

        private readonly IExecuterDispatcher _dispatcher;

        private readonly ICache _issueNumberCache;

        private readonly ICache _sportsEventCache;

        private readonly ICache _loggeryVenderMappingCache;

        public OrderingMessageHandler(ICacheManager cacheManager, IMessagePublisher publisher, IExecuterDispatcher dispatcher)
        {
            _issueNumberCache = cacheManager.GetCache("IssueNumbers");
            _sportsEventCache = cacheManager.GetCache("SportsEvents");
            _loggeryVenderMappingCache = cacheManager.GetCache("LotteryVenderMappings");
            _publisher = publisher;
            _dispatcher = dispatcher;
        }

        public async Task<bool> Handle(OrderingMessage message, CancellationToken token)
        {
            if (!token.IsCancellationRequested)
            {
                var ldpVenderId = await _loggeryVenderMappingCache.GetAsync($"{message.LvpVenderId}-{message.LotteryId}", (key) => { return "800"; });

                OrderingExecuter executer = new OrderingExecuter(message.LvpVenderId, ldpVenderId)
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
                        LdpVenderId = ldpVenderId,
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
