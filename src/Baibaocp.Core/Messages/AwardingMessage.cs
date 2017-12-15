using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.Core.Messages
{
    public class AwardingMessage
    {
        public string OrderId { get; set; }

        public string LvpVenderId { get; set; }

        public string LdpVenderId { get; set; }

        public int LotteryId { get; set; }

        public string TicketOdds { get; set; }

        public int Amount { get; set; }

        public int AftertaxAmount { get; set; }

        public OrderStatus Status { get; set; }
    }
}
