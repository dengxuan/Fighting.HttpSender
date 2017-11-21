using Baibaocp.LotteryVender.Abstractions;
using Fighting.Extensions.Messaging.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryVender.Messaging
{
    public class OrderingMessageHandler : IMessageHandler<OrderingMessage>
    {
        private readonly IExecuterDispatcher _executerDispatcher;

        public OrderingMessageHandler(IExecuterDispatcher executerDispatcher)
        {
            _executerDispatcher = executerDispatcher;
        }

        public async Task Handle(OrderingMessage message, CancellationToken token)
        {
            if (!token.IsCancellationRequested)
            {
                Console.WriteLine($"{message.Id}, {message.InvestCode}");
            }
            await Task.CompletedTask;
        }
    }
}
