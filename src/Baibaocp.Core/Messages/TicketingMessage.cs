namespace Baibaocp.Core.Messages
{
    public class TicketingMessage
    {
        public string OrderId { get; set; }

        public string LvpVenderId { get; set; }

        public string LvpOrderId { get; set; }

        public string LdpVenderId { get; set; }

        public string TicketOdds { get; set; }

        public int TicketStatus { get; set; }
    }
}
