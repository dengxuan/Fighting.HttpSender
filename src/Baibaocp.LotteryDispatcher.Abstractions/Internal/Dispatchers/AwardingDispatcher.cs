using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.Executers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Internal.Dispatchers
{
    public class AwardingDispatcher : IExecuterDispatcher<AwardingExecuter>
    {
        private readonly ILogger _logger;

        private readonly IServiceProvider _resolver;

        private readonly LotteryDispatcherOptions _options;

        public AwardingDispatcher(IServiceProvider resolver, ILogger<OrderingDispatcher> logger, LotteryDispatcherOptions options)
        {
            _logger = logger;
            _options = options;
            _resolver = resolver;
        }

        public async Task<bool> DispatchAsync(AwardingExecuter executer)
        {
            var handlerType = _options.GetHandler<AwardingExecuter>(executer.LdpVenderId);
            var handler = (IExecuteHandler<AwardingExecuter>)_resolver.GetRequiredService(handlerType);
            var result = await handler.HandleAsync(executer);
            return result;
        }
    }
}
