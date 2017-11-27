﻿using System.ComponentModel.DataAnnotations;

namespace Baibaocp.LotteryVender.WebApi.Entity
{
    /// <summary>
    /// 用户订单
    /// </summary>
    public class OrderInput
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Required]
        public string OrderId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [Required]
        public long? UserId { get; set; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        [Required]
        public int LotteryId { get; set; }

        /// <summary>
        /// 玩法编号
        /// </summary>
        [Required]
        public int LotteryPlayId { get; set; }

        /// <summary>
        /// 期号
        /// </summary>
        public int? IssueNumber { get; set; }

        /// <summary>
        /// 投注码
        /// </summary>
        [Required]
        public string InvestCode { get; set; }

        /// <summary>
        /// 投注类型
        /// </summary>
        [Required]
        public int InvestType { get; set; }

        /// <summary>
        /// 注数
        /// </summary>
        [Required]
        public sbyte InvestCount { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        [Required]
        public sbyte InvestTimes { get; set; }

        /// <summary>
        /// 投注金额
        /// </summary>
        [Required]
        public int InvestAmount { get; set; }
    }

}
