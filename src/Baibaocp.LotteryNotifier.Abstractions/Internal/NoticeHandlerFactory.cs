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

        private readonly ConcurrentDictionary<Type, object> _objectFactorys = new ConcurrentDictionary<Type, object>();

        public NoticeHandlerFactory(IServiceProvider iocResolver)
        {
            _iocResolver = iocResolver;
        }

        public INoticeHandler<TNotifier> GetHandler<TNotifier>(NoticeConfiguration configure) where TNotifier : INotifier
        {
            var handler = _objectFactorys.GetOrAdd(typeof(TNotifier), (key) =>
            {
                return _iocResolver.GetRequiredService<INoticeHandler<TNotifier>>();
                //ObjectFactory objectFactory = ActivatorUtilities.CreateFactory(typeof(INoticeHandler<TNotifier>), new Type[] { typeof(NoticeConfiguration) });
                //object @object = objectFactory.Invoke(_iocResolver, new object[] { configure });
                //return @object;
            });
            return handler as INoticeHandler<TNotifier>;
        }
    }
}
