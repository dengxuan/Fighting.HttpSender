using Baibaocp.LotteryDispatcher.Abstractions;

namespace Baibaocp.LotteryDispatcher.Executers
{
    public class AwardingExecuter : Executer
    {
        internal AwardingExecuter(string ldpVenderId) : base(ldpVenderId)
        {
        }

        public string OrderId { get; set; }
    }
}
