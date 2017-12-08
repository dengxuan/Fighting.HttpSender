using System;

namespace Baibaocp.LotteryVender.WebApi.Models
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public class OrderOutput
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        public int LotteryId { get; set; }

        /// <summary>
        /// 玩法编号
        /// </summary>
        public int LotteryPlayId { get; set; }

        /// <summary>
        /// 期号
        /// </summary>
        public int? IssueNumber { get; set; }

        /// <summary>
        /// 投注码
        /// </summary>
        public string InvestCode { get; set; }

        /// <summary>
        /// 出票赔率
        /// </summary>
        public string TicketOdds { get; set; }

        /// <summary>
        /// 投注类型
        /// </summary>
        public int InvestType { get; set; }

        /// <summary>
        /// 注数
        /// </summary>
        public int InvestCount { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        public int InvestTimes { get; set; }

        /// <summary>
        /// 投注金额
        /// </summary>
        public int InvestAmount { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        public int? BonusAmount { get; set; }

        /// <summary>
        /// 税后奖金
        /// </summary>
        public int? AftertaxBonusAmount { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
