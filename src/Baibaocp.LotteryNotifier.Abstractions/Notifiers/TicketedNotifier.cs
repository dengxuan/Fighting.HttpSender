using Baibaocp.LotteryNotifier.Abstractions;

namespace Baibaocp.LotteryNotifier.Notifiers
{
    public class TicketedNotifier : INotifier
    {
        public string OrderId { get; set; }

        public string LvpVenderId { get; set; }
    }
}
