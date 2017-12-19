using System.Collections.Generic;

namespace Baibaocp.Core.Messages
{
    /// <summary>
    /// 待返奖订单
    /// </summary>
    public class AwardingMessage
    {
        public string LdpOrderId { get; set; }

        public string LdpVenderId { get; set; }

        public List<OrderingMessage> LvpOrders { get; set; }
    }
}
