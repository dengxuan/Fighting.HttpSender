using Baibaocp.LotteryCommand.Abstractions;

namespace Baibaocp.LotteryVender.Sending.Shanghai
{
    public class OrderingCommand : ICommand
    {
        public string OrderId { get; set; }

        public int LotteryId { get; set; }

        public int LotteryPlayId { get; set; }

        public int IssueNumber { get; set; }

        public string InvestCode { get; set; }

        public int InvestAmount { get; set; }

        public int InvestCount { get; set; }

        public int InvestTimes { get; set; }

        public bool InvestType { get; set; }
    }
}
