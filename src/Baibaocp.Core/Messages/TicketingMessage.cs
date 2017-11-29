using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.Core.Messages
{
    public class TicketingMessage
    {
        public string OrderId { get; set; }

        public string LvpOrderId { get; set; }

        public string LvpVenderId { get; set; }

        public string LdpVenderId { get; set; }

        public int TicketStatus { get; set; }
    }
}
