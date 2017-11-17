using Baibaocp.LotteryCommand.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.LotteryCommand
{
    public class CommandSender : ICommandSender
    {
        private readonly IServiceProvider _resolver;

        public CommandSender(IServiceProvider resolver)
        {
            _resolver = resolver;
        }

        public async Task<ExecuteResult> SendAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand
        {
            var commandHandler = GetHandler<ICommandHandlerAsync<TCommand>, TCommand>(command);

            var executeResult = await commandHandler.HandleAsync(command);

            return executeResult;
        }

        private THandler GetHandler<THandler, TCommand>(TCommand command) where TCommand : ICommand 
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var commandHandler = _resolver.GetServices<THandler>();
            if (commandHandler == null)
                throw new Exception($"No handler found for command '{command.GetType().FullName}'");

            return commandHandler.First();
        }
    }
}
