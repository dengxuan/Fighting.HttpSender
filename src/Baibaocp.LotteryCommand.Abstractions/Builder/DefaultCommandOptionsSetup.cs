using Microsoft.Extensions.Options;

namespace Baibaocp.LotteryCommand.Builder
{
    internal class DefaultCommandOptionsSetup : ConfigureOptions<CommandOptions>
    {
        public DefaultCommandOptionsSetup()
            : base(ConfigureOptions)
        {
        }

        private static void ConfigureOptions(CommandOptions options)
        {
        }
    }
}
