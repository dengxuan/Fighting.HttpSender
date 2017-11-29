using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.Core.Messages
{
    public class AwardingMessage
    {
        public string OrderId { get; set; }

        public string LvpOrderId { get; set; }

        public string LvpVenderId { get; set; }

        public string LdpVenderId { get; set; }

        public string TicketOdds { get; set; }

        public int Amount { get; set; }

        public int AwardStatus { get; set; }
    }
}
