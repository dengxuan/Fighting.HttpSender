using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Abstractions
{

    public interface IExecuteHandler<in TExecuter> where TExecuter : IExecuter
    {

        Task<bool> HandleAsync(TExecuter executer);
    }
}