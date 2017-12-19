using Baibaocp.Core.Messages;
using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.Notifiers;
using Fighting.Extensions.Messaging.Abstractions;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Internal.MessageHandlers
{
    public class AwardedMessageHandler : IMessageHandler<AwardedMessage>
    {
        private readonly INoticeDispatcher _dispatcher;

        private readonly ILogger<AwardedMessageHandler> _logger;

        public AwardedMessageHandler(INoticeDispatcher dispatcher, ILogger<AwardedMessageHandler> logger)
        {
            _logger = logger;
            _dispatcher = dispatcher;
        }

        public async Task<bool> Handle(AwardedMessage message, CancellationToken token)
        {
            bool result = await _dispatcher.DispatchAsync(new Notifier<Awarded>(message.LvpVenderId)
            {
                Notice = new Awarded
                {
                    OrderId = message.LvpOrderId,
                    Status = 10400,
                    Amount = 20
                }
            });
            return result;
        }
    }
}
