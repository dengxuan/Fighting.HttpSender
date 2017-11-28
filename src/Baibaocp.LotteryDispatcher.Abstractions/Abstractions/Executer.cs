namespace Baibaocp.LotteryDispatcher.Abstractions
{
    public abstract class Executer : IExecuter
    {
        internal Executer(string lvpVenderId, string ldpVenderId)
        {
            LvpVenderId = lvpVenderId;
            LdpVenderId = ldpVenderId;
        }

        public string LvpVenderId { get; }

        public string LdpVenderId { get; }
    }
}
