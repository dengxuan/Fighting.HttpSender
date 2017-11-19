using System.Threading.Tasks;

namespace Baibaocp.LvpApi.Abstractions
{
    public interface IExecuterDispatcher
    {
        Task<ExecuteResult> DispatchAsync<TExecuter>(TExecuter executer) where TExecuter : IExecuter;
    }
}
