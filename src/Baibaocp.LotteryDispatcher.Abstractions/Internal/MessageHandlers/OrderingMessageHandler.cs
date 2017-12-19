using Baibaocp.Core.Messages;
using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.Executers;
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

        private readonly IExecuterDispatcher<OrderingExecuter> _dispatcher;

        private readonly ICache _issueNumberCache;

        private readonly ICache _sportsEventCache;

        private readonly ICache _loggeryVenderMappingCache;

        public OrderingMessageHandler(ICacheManager cacheManager, IMessagePublisher publisher, IExecuterDispatcher<OrderingExecuter> dispatcher)
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

                OrderingExecuter executer = new OrderingExecuter(ldpVenderId, new List<OrderingMessage> { message });
                return await _dispatcher.DispatchAsync(executer);
            }
            return false;
        }

    }
}
