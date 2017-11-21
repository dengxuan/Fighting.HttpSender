using System.Threading.Tasks;

namespace Baibaocp.LotteryVender.Abstractions
{
    public interface IExecuterDispatcher
    {
        Task<ExecuteResult> DispatchAsync<TExecuter>(TExecuter executer) where TExecuter : IExecuter;
    }
}
