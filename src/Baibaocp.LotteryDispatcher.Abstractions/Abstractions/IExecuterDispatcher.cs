using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Abstractions
{
    public interface IExecuterDispatcher<TExecuter> where TExecuter : IExecuter
    {
        Task<bool> DispatchAsync(TExecuter executer);
    }
}
