using Microsoft.Extensions.Options;

namespace Baibaocp.LotteryDispatcher.Builder
{
    internal class DefaultLvpApiOptionsSetup : ConfigureOptions<ExecuterOptions>
    {
        public DefaultLvpApiOptionsSetup()
            : base(ConfigureOptions)
        {
        }

        private static void ConfigureOptions(ExecuterOptions options)
        {
        }
    }
}
