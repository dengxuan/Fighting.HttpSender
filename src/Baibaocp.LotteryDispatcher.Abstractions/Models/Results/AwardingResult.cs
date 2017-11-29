using Baibaocp.LotteryDispatcher.Abstractions;

namespace Baibaocp.LotteryDispatcher.Models.Results
{
    public class AwardingResult : Result
    {
        public int? Amount { get; set; }

        public AwardingResult(int code) : base(code)
        {
        }

        public AwardingResult(int code, string message) : base(code, message)
        {
        }
    }
}
