using System.Threading.Tasks;

namespace Baibaocp.LvpApi.Abstractions
{

    public interface IExecuteHandler<in TExecuter> where TExecuter : IExecuter
    {

        Task<ExecuteResult> HandleAsync(TExecuter executer);
    }
}