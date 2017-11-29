using Baibaocp.LotteryDispatcher.Abstractions;

namespace Baibaocp.LotteryDispatcher.Models.Results
{
    public class OrderingResult : Result
    {
        public OrderingResult(int code) : base(code)
        {
        }

        public OrderingResult(int code, string message) : base(code, message)
        {
        }
    }
}
