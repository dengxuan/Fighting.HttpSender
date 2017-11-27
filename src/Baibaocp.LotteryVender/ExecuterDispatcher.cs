using Baibaocp.LotteryVender.Abstractions;
using Fighting.Extensions.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryVender
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
            var executeHandler = await GetHandlerAsync<IExecuteHandler<TExecuter>, TExecuter>(executer);

            var executeResult = await executeHandler.HandleAsync(executer);

            return executeResult;
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
    }
}
