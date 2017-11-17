namespace Baibaocp.LotteryVender.Sending.Abstractions
{
    public interface IOrder
    {
        /// <summary>
        /// 订单号
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        int LotteryId { get; set; }

        /// <summary>
        /// 玩法编号
        /// </summary>
        int LotteryPlayId { get; set; }

        /// <summary>
        /// 期号
        /// </summary>
        int? IssueNumber { get; set; }

        /// <summary>
        /// 投注码
        /// </summary>
        string InvestCode { get; set; }

        /// <summary>
        /// 投注类型
        /// </summary>
        bool InvestType { get; set; }

        /// <summary>
        /// 注数
        /// </summary>
        int InvestCount { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        int InvestTimes { get; set; }

        /// <summary>
        /// 投注金额
        /// </summary>
        int InvestAmount { get; set; }

    }
}
