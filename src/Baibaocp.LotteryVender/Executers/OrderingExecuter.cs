using Baibaocp.LotteryVender.Abstractions;

namespace Baibaocp.LotteryVender.Executers
{
    public class OrderingExecuter : IExecuter
    {

        public string OrderId { get; set; }

        public int LotteryId { get; set; }

        public int LotteryPlayId { get; set; }

        public int? IssueNumber { get; set; }

        public sbyte InvestCount { get; set; }

        public sbyte InvestTimes { get; set; }

        public int InvestAmount { get; set; }

        public bool InvestType { get; set; }

        public string InvestCode { get; set; }
    }
}
