using Baibaocp.LotteryDispatcher.Abstractions;

namespace Baibaocp.LotteryDispatcher.Models.Results
{
    public class TicketingResult : Result
    {
        public TicketingResult(int code) : base(code)
        {
        }

        public TicketingResult(int code, string message) : base(code, message)
        {
        }

        public string TicketOdds { get; set; }
    }
}
