using Baibaocp.Core;
using Baibaocp.Core.Messages;
using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.Executers;
using Baibaocp.LotteryDispatcher.Models.Results;
using Fighting.Extensions.Caching.Abstractions;
using Fighting.Extensions.Messaging.Abstractions;
using Fighting.Storaging.Abstractions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Internal.MessageHandlers
{
    public class OrderingMessageHandler : IMessageHandler<OrderingMessage>
    {

        private readonly IMessagePublisher _publisher;

        private readonly IExecuterDispatcher _dispatcher;

        private readonly IStringGenerator _stringGenerator;

        private readonly ICache _issueNumberCache;

        private readonly ICache _sportsEventCache;

        private readonly ICache _loggeryVenderMappingCache;

        public OrderingMessageHandler(ICacheManager cacheManager, IMessagePublisher publisher, IExecuterDispatcher dispatcher, IStringGenerator stringGenerator)
        {
            _issueNumberCache = cacheManager.GetCache("IssueNumbers");
            _sportsEventCache = cacheManager.GetCache("SportsEvents");
            _loggeryVenderMappingCache = cacheManager.GetCache("LotteryVenderMappings");
            _publisher = publisher;
            _dispatcher = dispatcher;
            _stringGenerator = stringGenerator;
        }

        public async Task<bool> Handle(OrderingMessage message, CancellationToken token)
        {
            if (!token.IsCancellationRequested)
            {
                var ldpVenderId = await _loggeryVenderMappingCache.GetAsync($"{message.LvpVenderId}-{message.LotteryId}", (key) => { return "800"; });

                OrderingExecuter executer = new OrderingExecuter(ldpVenderId)
                {
                    OrderId = _stringGenerator.Create(),
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
                    if (executeResult.Result.Code == OrderStatus.Ticketing || executeResult.Result.Code == OrderStatus.TicketDrawing)
                    {
                        message.Status = executeResult.Result.Code;
                        TicketingMessage ticketingMessage = new TicketingMessage
                        {
                            LdpOrderId = executer.OrderId,
                            LdpVenderId = ldpVenderId,
                            LvpOrders = new List<OrderingMessage>
                            {
                                message
                            }
                        };
                        await _publisher.Publish(RoutingkeyConsts.Orders.Completed.Success, ticketingMessage, token);
                    }
                    else
                    {
                        await _publisher.Publish(RoutingkeyConsts.Orders.Completed.Failure, new
                        {
                            LvpOrderId = message.LvpOrderId,
                            LvpVenderId = message.LvpVenderId,
                            Amount = message.InvestAmount,
                            Status = executeResult.Result.Code
                        }, token);
                    }
                }
                else
                {
                    await _publisher.Publish(RoutingkeyConsts.Orders.Completed.Failure, new
                    {
                        LvpOrderId = message.LvpOrderId,
                        LvpVenderId = message.LvpVenderId,
                        Amount = message.InvestAmount,
                        Status = OrderStatus.TicketNotRecv
                    }, token);
                }
            }
            return true;
        }
    }
}
