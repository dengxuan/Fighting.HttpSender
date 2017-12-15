using System.Collections.Generic;

namespace Baibaocp.Core.Messages
{
    /// <summary>
    /// 待出票订单
    /// </summary>
    public class TicketingMessage
    {
        public string LdpOrderId { get; set; }

        public string LdpVenderId { get; set; }

        public List<OrderingMessage> LvpOrders { get; set; }
    }
}
