using System.Threading.Tasks;

namespace Baibaocp.LotteryCommand.Abstractions
{
    public interface ICommandSender
    {

        Task<ExecuteResult> SendAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand;
    }
}
