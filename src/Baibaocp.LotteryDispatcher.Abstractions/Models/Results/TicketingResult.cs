using Baibaocp.Core;
using Baibaocp.LotteryDispatcher.Abstractions;

namespace Baibaocp.LotteryDispatcher.Models.Results
{
    public class TicketingResult : Result
    {
        public TicketingResult(OrderStatus code) : base(code)
        {
        }

        public TicketingResult(OrderStatus code, string message) : base(code, message)
        {
        }

        public string TicketOdds { get; set; }
    }
}
