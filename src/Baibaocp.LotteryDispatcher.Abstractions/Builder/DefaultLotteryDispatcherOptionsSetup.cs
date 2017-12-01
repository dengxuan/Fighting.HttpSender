using Microsoft.Extensions.Options;

namespace Baibaocp.LotteryDispatcher.Builder
{
    internal class DefaultLotteryDispatcherOptionsSetup : ConfigureOptions<LotteryDispatcherOptions>
    {
        public DefaultLotteryDispatcherOptionsSetup()
            : base(ConfigureOptions)
        {
        }

        private static void ConfigureOptions(LotteryDispatcherOptions options)
        {
        }
    }
}
