using System.Threading.Tasks;

namespace Baibaocp.LotteryCommand.Abstractions
{

    public interface ICommandHandlerAsync<in TCommand> where TCommand : ICommand
    {
        Task<ExecuteResult> HandleAsync(TCommand command);
    }
}