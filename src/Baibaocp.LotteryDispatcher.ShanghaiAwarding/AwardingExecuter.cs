using Baibaocp.LotteryDispatcher.Abstractions;

namespace Baibaocp.LotteryDispatcher.ShanghaiAwarding
{
    public class AwardingExecuter : Executer
    {
        public AwardingExecuter(string lvpVenderId, string ldpVenderId) : base(lvpVenderId, ldpVenderId)
        {
        }

        public string OrderId { get; set; }
    }
}
