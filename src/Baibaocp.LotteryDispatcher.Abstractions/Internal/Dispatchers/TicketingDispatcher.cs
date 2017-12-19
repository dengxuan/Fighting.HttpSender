using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.Executers;
using Fighting.Extensions.Messaging.Abstractions;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Internal.Dispatchers
{
    public class TicketingDispatcher : IExecuterDispatcher<TicketingExecuter>
    {

        private readonly IServiceProvider _resolver;

        private readonly LotteryDispatcherOptions _options;

        private readonly ILogger<OrderingDispatcher> _logger;

        private readonly IExecuterDispatcher<AwardingExecuter> _awardingDispatcher;

        public TicketingDispatcher(IServiceProvider resolver, LotteryDispatcherOptions options, ILogger<OrderingDispatcher> logger, IExecuterDispatcher<AwardingExecuter> awardingDispatcher)
        {
            _logger = logger;
            _options = options;
            _resolver = resolver;
            _awardingDispatcher = awardingDispatcher;
        }

        public async Task<bool> DispatchAsync(TicketingExecuter executer)
        {
            var handlerType = _options.GetHandler<TicketingExecuter>(executer.LdpVenderId);
            var handler = (IExecuteHandler<TicketingExecuter>)_resolver.GetRequiredService(handlerType);
            var result = await handler.HandleAsync(executer);
            try
            {
                if (result)
                {
                    /* 添加开奖查询后台任务，开奖后执行查询，主动查询出票结果*/
                    BackgroundJob.Schedule(() => _awardingDispatcher.DispatchAsync(new AwardingExecuter(executer.LdpVenderId, executer.LdpOrderId, executer.LvpOrders.ToList())), TimeSpan.FromSeconds(30));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Dispatcher error : {0}", ex);
            }
            return result;
        }
    }
}
