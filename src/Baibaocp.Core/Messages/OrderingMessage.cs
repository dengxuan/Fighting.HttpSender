using ProtoBuf;
using System;

namespace Baibaocp.LotteryVender.Messaging
{
    /// <summary>
    /// 待投注消息
    /// </summary>
    [ProtoContract]
    public class OrderingMessage
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [ProtoMember(1)]
        public string OrderId { get; set; }

        /// <summary>
        /// 下游渠道订单号
        /// </summary>
        public string LvpOrderId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [ProtoMember(2)]
        public long? LvpUserId { get; set; }

        /// <summary>
        /// 渠道编号
        /// </summary>
        [ProtoMember(3)]
        public string LvpVenderId { get; set; }

        /// <summary>
        /// 上游渠道编号
        /// </summary>
        public string LdpVenderId { get; set; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        [ProtoMember(4)]
        public int LotteryId { get; set; }

        /// <summary>
        /// 玩法编号
        /// </summary>
        [ProtoMember(5)]
        public int LotteryPlayId { get; set; }

        /// <summary>
        /// 期号
        /// </summary>
        [ProtoMember(6)]
        public int? IssueNumber { get; set; }

        /// <summary>
        /// 投注码
        /// </summary>
        [ProtoMember(7)]
        public string InvestCode { get; set; }

        /// <summary>
        /// 投注类型
        /// </summary>
        [ProtoMember(8)]
        public bool InvestType { get; set; }

        /// <summary>
        /// 注数
        /// </summary>
        [ProtoMember(9)]
        public sbyte InvestCount { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        [ProtoMember(10)]
        public sbyte InvestTimes { get; set; }

        /// <summary>
        /// 投注金额
        /// </summary>
        [ProtoMember(11)]
        public int InvestAmount { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [ProtoMember(12)]
        public DateTime CreationTime { get; set; }
    }
}
