using Baibaocp.LotteryDispatcher.Abstractions;
using Fighting.Extensions.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher
{
    public class ExecuterDispatcher : IExecuterDispatcher
    {

        private readonly IServiceProvider _resolver;

        private readonly ExecuterOptions _options;

        private readonly IMessagePublisher _publisher;

        public ExecuterDispatcher(IServiceProvider resolver, IMessagePublisher publisher, ExecuterOptions options)
        {
            _resolver = resolver;
            _publisher = publisher;
            _options = options;
        }


        public async Task<ExecuteResult> DispatchAsync<TExecuter>(TExecuter executer) where TExecuter : IExecuter
        {
            var handlerTypes = _options.GetHandlerTypes(executer.LvpVenderId);
            foreach (var handlerType in handlerTypes)
            {
                var handler = await GetHandlerAsync<TExecuter>(handlerType);
                var result = await handler.HandleAsync(executer);
                if (result.Success)
                {
                    return result;
                }
            }

            return new ExecuteResult(false);
        }

        private Task<THandler> GetHandlerAsync<THandler, TExecuter>(TExecuter executer) where THandler : IExecuteHandler<TExecuter> where TExecuter : IExecuter
        {
            if (executer == null)
            {
                throw new ArgumentNullException(nameof(executer));
            }
            var handler = _resolver.GetRequiredService<THandler>();
            if (handler == null)
            {
                throw new Exception($"No handler found for executer '{executer.GetType().FullName}'");
            }
            return Task.FromResult(handler); ;
        }

        private Task<IExecuteHandler<TExecuter>> GetHandlerAsync<TExecuter>(Type handlerType) where TExecuter : IExecuter
        {
            var handler = (IExecuteHandler<TExecuter>)_resolver.GetRequiredService(handlerType);
            if (handler == null)
            {
                throw new Exception($"No handler found '{handlerType.FullName}'");
            }
            return Task.FromResult(handler); ;
        }
    }
}
