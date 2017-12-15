using Baibaocp.Core;
using Baibaocp.Core.Messages;
using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.Executers;
using Baibaocp.LotteryDispatcher.Models.Results;
using Fighting.Extensions.Messaging.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.ShanghaiAwarding.Handlers
{
    public class AwardingMessageHandler : IMessageHandler<AwardingMessage>
    {

        private readonly IMessagePublisher _publisher;

        private readonly IExecuterDispatcher _dispatcher;

        public AwardingMessageHandler(IMessagePublisher publisher, IExecuterDispatcher dispatcher)
        {
            _publisher = publisher;
            _dispatcher = dispatcher;
        }

        public async Task<bool> Handle(AwardingMessage message, CancellationToken token)
        {
            if (!token.IsCancellationRequested)
            {
                AwardingExecuter executer = new AwardingExecuter(message.LdpVenderId)
                {
                    OrderId = message.OrderId,
                };
                var executeResult = await _dispatcher.DispatchAsync<AwardingExecuter, AwardingResult>(executer);
                if (executeResult.Success)
                {
                    message.Status = executeResult.Result.Code;
                    message.Amount = executeResult.Result.Amount ?? 0;
                    if (executeResult.Result.Code == OrderStatus.TicketWinning)
                    {
                        await _publisher.Publish(RoutingkeyConsts.Awards.Completed.Winning, message, token);
                        return true;
                    }
                    else if (executeResult.Result.Code == OrderStatus.TicketLosing)
                    {
                        await _publisher.Publish(RoutingkeyConsts.Awards.Completed.Loseing, message, token);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
