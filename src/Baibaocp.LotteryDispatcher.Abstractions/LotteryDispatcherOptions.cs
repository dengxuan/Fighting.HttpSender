using Baibaocp.LotteryDispatcher.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Baibaocp.LotteryDispatcher
{
    public class LotteryDispatcherOptions
    {
        private readonly ConcurrentDictionary<(string ldpVenderId, Type executerType), Type> _ldpHandlerTypesMapping = new ConcurrentDictionary<(string ldpVenderId, Type executerType), Type>();

        public void AddHandler<THandler, TExecuter, TResult>(string ldpVenderId) where THandler : IExecuteHandler<TExecuter, TResult> where TExecuter : IExecuter where TResult : IResult
        {
            _ldpHandlerTypesMapping.TryAdd((ldpVenderId, typeof(TExecuter)), typeof(THandler));
        }

        internal Type GetHandler<TExecuter>(string ldpVenderId)
        {
            _ldpHandlerTypesMapping.TryGetValue((ldpVenderId, typeof(TExecuter)), out Type value);
            return value;
        }

        internal List<string> GetLdpVenderId<TExecuter>()
        {
            return _ldpHandlerTypesMapping.Keys.Where(predicate => predicate.executerType == typeof(TExecuter)).Select(selector => selector.ldpVenderId).ToList();
        }
    }
}
