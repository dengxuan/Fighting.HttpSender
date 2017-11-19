using Baibaocp.LvpApi.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LvpApi
{
    public class ExecuteHandlerFactory : IExecuteHandlerFactory
    {
        private readonly IServiceProvider _iocResolver;

        public ExecuteHandlerFactory(IServiceProvider iocResolver)
        {
            _iocResolver = iocResolver;
        }

        public Task<IExecuteHandler<TExecuter>> GetHandlerAsync<TExecuter>(string lvpVenderId) where TExecuter : IExecuter
        {
            var executeHandlers = _iocResolver.GetServices<IExecuteHandler<TExecuter>>();
            if (executeHandlers == null)
            {
                throw new Exception($"No handler found for executer '{ typeof(TExecuter).Name}'");
            }
            foreach (var handler in executeHandlers)
            {
                return Task.FromResult(handler);
            }
            throw new Exception($"No handler support for vender '{lvpVenderId}'");
        }
    }
}
