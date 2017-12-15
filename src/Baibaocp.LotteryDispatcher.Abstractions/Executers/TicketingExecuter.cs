using Baibaocp.LotteryDispatcher.Abstractions;

namespace Baibaocp.LotteryDispatcher.Executers
{
    public class TicketingExecuter : Executer
    {
        internal TicketingExecuter(string ldpVenderId) : base(ldpVenderId)
        {
        }

        public string OrderId { get; set; }
    }
}
