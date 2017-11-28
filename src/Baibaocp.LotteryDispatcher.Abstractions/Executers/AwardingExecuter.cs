using Baibaocp.LotteryDispatcher.Abstractions;

namespace Baibaocp.LotteryDispatcher.Executers
{
    public class AwardingExecuter : Executer
    {
        internal AwardingExecuter(string lvpVenderId, string ldpVenderId) : base(lvpVenderId, ldpVenderId)
        {
        }

        public string OrderId { get; set; }
    }
}
