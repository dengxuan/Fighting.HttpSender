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
                TicketingExecuter executer = new TicketingExecuter(message.LvpVenderId, message.LdpVenderId)
                {
                    OrderId = message.OrderId,
                };
                var executeResult = await _dispatcher.DispatchAsync<TicketingExecuter, TicketingResult>(executer);
                if (executeResult.Success)
                {
                    AwardingMessage ticketedMessage = new AwardingMessage
                    {
                        OrderId = message.OrderId,
                        LvpOrderId = message.LvpOrderId,
                        LvpVenderId = message.LvpVenderId,
                        LdpVenderId = message.LdpVenderId,
                        TicketOdds = executeResult.Result.TicketOdds,
                        AwardStatus = executeResult.Result.Code
                    };
                    if (executeResult.Result.Code == OrderStatus.Ticketing.Success)
                    {
                        await _publisher.Publish(RoutingkeyConsts.Tickets.Completed.Success, ticketedMessage, token);
                        return true;
                    }
                    else if (executeResult.Result.Code == OrderStatus.Ticketing.Failure)
                    {
                        await _publisher.Publish(RoutingkeyConsts.Tickets.Completed.Failure, ticketedMessage, token);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
