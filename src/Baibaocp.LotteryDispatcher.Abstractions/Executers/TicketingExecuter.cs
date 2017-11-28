using Baibaocp.LotteryDispatcher.Abstractions;

namespace Baibaocp.LotteryDispatcher.Executers
{
    public class TicketingExecuter : Executer
    {
        internal TicketingExecuter(string lvpVenderId, string ldpVenderId) : base(lvpVenderId, ldpVenderId)
        {
        }

        public string OrderId { get; set; }
    }
}
