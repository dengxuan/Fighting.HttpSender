using Baibaocp.LotteryDispatcher.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Baibaocp.LotteryDispatcher
{
    public class ExecuterOptions
    {
        private readonly ConcurrentDictionary<string, ISet<Type>> _lvpHandlerTypesMapping = new ConcurrentDictionary<string, ISet<Type>>();

        public void AddHandler<THandler, TExecuter>(string lvpVenderId) where THandler : IExecuteHandler<TExecuter> where TExecuter : IExecuter
        {
            ISet<Type> types = _lvpHandlerTypesMapping.GetOrAdd(lvpVenderId, (key) =>
            {
                return new HashSet<Type>();
            });
            types.Add(typeof(THandler));
        }

        internal IReadOnlyList<Type> GetHandlerTypes(string lvpVenderId)
        {
            _lvpHandlerTypesMapping.TryGetValue(lvpVenderId, out ISet<Type> value);
            return value.ToList();
        }
    }
}
