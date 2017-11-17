using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Weapsy.Framework.Queries
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _resolver;

        public QueryDispatcher(IServiceProvider resolver)
        {
            _resolver = resolver;
        }

        public TResult Dispatch<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            var queryHandler = GetHandler<IQueryHandler<TQuery, TResult>, TQuery>(query);

            return queryHandler.Retrieve(query);
        }

        public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            var queryHandler = GetHandler<IQueryHandlerAsync<TQuery, TResult>, TQuery>(query);

            return await queryHandler.RetrieveAsync(query);
        }

        private THandler GetHandler<THandler, TQuery>(TQuery query) where TQuery : IQuery
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var queryHandler = _resolver.GetService<THandler>();

            if (queryHandler == null)
                throw new Exception($"No handler found for query '{query.GetType().FullName}'");

            return queryHandler;
        }
    }
}