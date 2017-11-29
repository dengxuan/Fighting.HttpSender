using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Abstractions
{

    public interface IExecuteHandler<in TExecuter, TResult> where TExecuter : IExecuter where TResult : IResult
    {

        Task<TResult> HandleAsync(TExecuter executer);
    }
}