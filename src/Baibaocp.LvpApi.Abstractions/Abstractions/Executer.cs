namespace Baibaocp.LvpApi.Abstractions
{
    public abstract class Executer : IExecuter
    {
        public string VenderId { get; }

        public Executer(string venderId) => VenderId = venderId;
    }
}
