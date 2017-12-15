using Baibaocp.Core;
using Baibaocp.Core.Messages;
using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.Executers;
using Baibaocp.LotteryDispatcher.Models.Results;
using Fighting.Extensions.Messaging.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.ShanghaiTicketing.Handlers
{
    public class TicketingMessageHandler : IMessageHandler<TicketingMessage>
    {

        private readonly IMessagePublisher _publisher;

        private readonly IExecuterDispatcher _dispatcher;

        public TicketingMessageHandler(IMessagePublisher publisher, IExecuterDispatcher dispatcher)
        {
            _publisher = publisher;
            _dispatcher = dispatcher;
        }

        public async Task<bool> Handle(TicketingMessage message, CancellationToken token)
        {
            if (!token.IsCancellationRequested)
            {
                TicketingExecuter executer = new TicketingExecuter(message.LdpVenderId)
                {
                    OrderId = message.LdpOrderId,
                };
                var executeResult = await _dispatcher.DispatchAsync<TicketingExecuter, TicketingResult>(executer);
                if (executeResult.Success)
                {
                    if (executeResult.Result.Code == OrderStatus.TicketDrawing)
                    {
                        foreach (var lvpOrder in message.LvpOrders)
                        {
                            await _publisher.Publish(RoutingkeyConsts.Tickets.Completed.Success, new
                            {
                                LvpOrderId = lvpOrder.LvpOrderId,
                                LvpVenderId = lvpOrder.LvpVenderId,
                                LdpOrderId = message.LdpOrderId,
                                LdpVenderId = message.LdpVenderId,
                                Amount = lvpOrder.InvestAmount,
                                TicketOdds = executeResult.Result.TicketOdds,
                                Status = executeResult.Result.Code
                            }, token);
                        }
                        return true;
                    }
                    else if (executeResult.Result.Code == OrderStatus.TicketFailed)
                    {
                        foreach (var lvpOrder in message.LvpOrders)
                        {
                            await _publisher.Publish(RoutingkeyConsts.Tickets.Completed.Failure, new
                            {
                                LvpOrderId = lvpOrder.LvpOrderId,
                                LvpVenderId = lvpOrder.LvpVenderId,
                                LdpOrderId = message.LdpOrderId,
                                LdpVenderId = message.LdpVenderId,
                                Amount = lvpOrder.InvestAmount,
                                TicketOdds = executeResult.Result.TicketOdds,
                                Status = executeResult.Result.Code
                            }, token);
                        }
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
