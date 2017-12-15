using Baibaocp.LotteryNotifier.Abstractions;
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

        public async Task<bool> DispatchAsync<TNotice>(INotifier<TNotice> notifier) where TNotice : class
        {
            try
            {
                NoticeConfiguration configuration = _options.Configures.Where(predicate => predicate.LvpVenderId == notifier.LvpVenderId).SingleOrDefault();
                if(configuration == null)
                {
                    return true;
                }
                var handler = _handlerFactory.GetHandler<TNotice>(configuration);
                var result = await handler.HandleAsync(notifier.Notice);
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
