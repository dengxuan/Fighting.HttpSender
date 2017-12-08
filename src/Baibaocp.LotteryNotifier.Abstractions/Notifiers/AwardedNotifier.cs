using Baibaocp.LotteryNotifier.Abstractions;

namespace Baibaocp.LotteryNotifier.Notifiers
{
    public class AwardedNotifier : INotifier
    {
        public string OrderId { get; set; }

        public string LvpVenderId { get; set; }

        public int Status { get; set; }

        public int Amount { get; set; }
    }
}
