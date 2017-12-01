using Baibaocp.LotteryNotifier.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections;

namespace Baibaocp.LotteryNotifier.Internal
{
    internal class NoticeHandlerFactory : INoticeHandlerFactory
    {
        private readonly IServiceProvider _iocResolver;

        private readonly ConcurrentDictionary<Type, ObjectFactory> _objectFactorys = new ConcurrentDictionary<Type, ObjectFactory>();

        public NoticeHandlerFactory(IServiceProvider iocResolver)
        {
            _iocResolver = iocResolver;
        }

        public INoticeHandler<TNotifier> GetHandler<TNotifier>(NoticeConfiguration configure) where TNotifier : INotifier
        {
            var factory = _objectFactorys.GetOrAdd(typeof(TNotifier), (key) =>
            {
                ObjectFactory objectFactory = ActivatorUtilities.CreateFactory(typeof(INoticeHandler<TNotifier>), new Type[] { typeof(NoticeConfiguration) });
                return objectFactory;
            });
            object @object = factory.Invoke(_iocResolver, new object[] { configure });
            return @object as INoticeHandler<TNotifier>;
        }
    }
}
