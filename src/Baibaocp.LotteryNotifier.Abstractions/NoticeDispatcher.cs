using Baibaocp.LotteryNotifier.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier
{
    internal class NoticeDispatcher : INoticeDispatcher
    {
        private readonly ILogger _logger;

        private readonly LotteryNoticeOptions _options;

        private readonly INoticeHandlerFactory _handlerFactory;

        public NoticeDispatcher(LotteryNoticeOptions options, INoticeHandlerFactory handlerFactory, ILoggerFactory loggerFactory)
        {
            _options = options;
            _handlerFactory = handlerFactory;
            _logger = loggerFactory.CreateLogger<NoticeDispatcher>();
        }

        public async Task<bool> DispatchAsync<TNotifier>(TNotifier notifier) where TNotifier : INotifier
        {
            try
            {
                NoticeConfiguration configuration = _options.Configures.Where(predicate => predicate.LvpVenderId == notifier.LvpVenderId).SingleOrDefault();

                var handler = _handlerFactory.GetHandler<TNotifier>(configuration);
                var result = await handler.HandleAsync(notifier);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Dispatcher error : {0}", ex);
                return false;
            }
        }
    }
}
