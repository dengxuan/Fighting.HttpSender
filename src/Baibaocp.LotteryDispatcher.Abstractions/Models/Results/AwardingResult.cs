using Baibaocp.Core;
using Baibaocp.LotteryDispatcher.Abstractions;

namespace Baibaocp.LotteryDispatcher.Models.Results
{
    public class AwardingResult : Result
    {
        public int? Amount { get; set; }

        public AwardingResult(OrderStatus code) : base(code)
        {
        }

        public AwardingResult(OrderStatus code, string message) : base(code, message)
        {
        }
    }
}
