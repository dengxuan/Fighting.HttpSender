using Baibaocp.Core.Messages;
using Baibaocp.LotteryDispatcher.Abstractions;
using Fighting.Storaging;
using System.Collections.Generic;

namespace Baibaocp.LotteryDispatcher.Executers
{
    public class OrderingExecuter : Executer
    {
        internal OrderingExecuter(string ldpVenderId, List<OrderingMessage> lvpOrders) : base(ldpVenderId)
        {
            LdpOrderId = SequentialStringGenerator.Instance.Create();
            LvpOrders = lvpOrders;
        }

        public string LdpOrderId { get; }

        public IReadOnlyList<OrderingMessage> LvpOrders { get; }
    }
}
