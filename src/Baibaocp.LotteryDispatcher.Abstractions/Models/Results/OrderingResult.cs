using Baibaocp.Core;
using Baibaocp.LotteryDispatcher.Abstractions;

namespace Baibaocp.LotteryDispatcher.Models.Results
{
    public class OrderingResult : Result
    {
        public OrderingResult(OrderStatus code) : base(code)
        {
        }

        public OrderingResult(OrderStatus code, string message) : base(code, message)
        {
        }
    }
}
