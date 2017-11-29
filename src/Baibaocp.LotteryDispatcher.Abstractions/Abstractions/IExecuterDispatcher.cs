using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Abstractions
{
    public interface IExecuterDispatcher
    {
        Task<ExecuteResult<TResult>> DispatchAsync<TExecuter, TResult>(TExecuter executer) where TExecuter : IExecuter where TResult: IResult;
    }
}
