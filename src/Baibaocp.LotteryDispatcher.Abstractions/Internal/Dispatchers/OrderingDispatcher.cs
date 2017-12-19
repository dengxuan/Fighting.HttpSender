using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.Executers;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher
{
    public class OrderingDispatcher : IExecuterDispatcher<OrderingExecuter>
    {
        private readonly ILogger _logger;

        private readonly IServiceProvider _resolver;

        private readonly LotteryDispatcherOptions _options;

        private readonly IExecuterDispatcher<TicketingExecuter> _ticketDispatcher;

        public OrderingDispatcher(IServiceProvider resolver, ILogger<OrderingDispatcher> logger, LotteryDispatcherOptions options, IExecuterDispatcher<TicketingExecuter> ticketDispatcher)
        {
            _logger = logger;
            _options = options;
            _resolver = resolver;
            _ticketDispatcher = ticketDispatcher;
        }


        public async Task<bool> DispatchAsync(OrderingExecuter executer)
        {
            var handlerType = _options.GetHandler<OrderingExecuter>(executer.LdpVenderId);
            var handler = (IExecuteHandler<OrderingExecuter>)_resolver.GetRequiredService(handlerType);
            var result = await handler.HandleAsync(executer);
            try
            {
                if (result)
                {
                    /* 投注成功后延迟30秒，主动查询出票结果*/
                    BackgroundJob.Schedule(() => _ticketDispatcher.DispatchAsync(new TicketingExecuter(executer.LdpVenderId, executer.LdpOrderId, executer.LvpOrders.ToList())), TimeSpan.FromSeconds(30));
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
