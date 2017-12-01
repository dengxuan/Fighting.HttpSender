using Baibaocp.LotteryDispatcher.Abstractions;
using Fighting.Extensions.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher
{
    public class ExecuterDispatcher : IExecuterDispatcher
    {
        private readonly ILogger _logger;

        private readonly IServiceProvider _resolver;

        private readonly LotteryDispatcherOptions _options;

        private readonly IMessagePublisher _publisher;

        public ExecuterDispatcher(IServiceProvider resolver, IMessagePublisher publisher, LotteryDispatcherOptions options, ILogger<ExecuterDispatcher> logger)
        {
            _resolver = resolver;
            _publisher = publisher;
            _options = options;
            _logger = logger;
        }


        public async Task<ExecuteResult<TResult>> DispatchAsync<TExecuter, TResult>(TExecuter executer) where TExecuter : IExecuter where TResult : IResult
        {
            try
            {
                var ldpVenderIds = _options.GetLdpVenderId<TExecuter>();
                foreach (var ldpVenderId in ldpVenderIds)
                {
                    var handlerType = _options.GetHandler<TExecuter>(ldpVenderId);
                    var handler = await GetHandlerAsync<TExecuter, TResult>(handlerType);
                    var result = await handler.HandleAsync(executer);
                    return new ExecuteResult<TResult>(result) { VenderId = ldpVenderId };
                }
                return new ExecuteResult<TResult>(false);
            }
            catch (Exception ex)
            {
                _logger.LogError("Dispatcher error : {0}", ex);
                return new ExecuteResult<TResult>(false);
            }
        }

        private Task<IExecuteHandler<TExecuter, TResult>> GetHandlerAsync<TExecuter, TResult>(Type handlerType) where TExecuter : IExecuter where TResult : IResult
        {
            var handler = (IExecuteHandler<TExecuter, TResult>)_resolver.GetRequiredService(handlerType);
            if (handler == null)
            {
                throw new Exception($"No handler found '{handlerType.FullName}'");
            }
            return Task.FromResult(handler); ;
        }
    }
}
