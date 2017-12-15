namespace Baibaocp.Core.Messages
{
    public class TicketedMessage
    {
        public string LvpOrderId { get; set; }

        public string LvpVenderId { get; set; }

        public string LdpOrderId { get; set; }

        public string LdpVenderId { get; set; }

        public int LotteryId { get; set; }

        public int LotteryPlayId { get; set; }

        public int Amount { get; set; }

        public string TicketOdds { get; set; }

        public OrderStatus Status { get; set; }

    }
}
