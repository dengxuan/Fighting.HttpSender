namespace Baibaocp.Core
{
    public static class OrderStatus
    {
        /// <summary>
        /// 投注状态
        /// </summary>
        public static class Ordering
        {
            /// <summary>
            /// 等待投注
            /// </summary>
            public const int Waiting = 0x0001;

            /// <summary>
            /// 投注成功
            /// </summary>
            public const int Success = 0x0002;

            /// <summary>
            /// 投注失败
            /// </summary>
            public const int Failure = 0x0003;
        }

        /// <summary>
        /// 出票状态
        /// </summary>
        public static class Ticketing
        {
            /// <summary>
            /// 等待出票
            /// </summary>
            public const int Waiting = 0x0010;

            /// <summary>
            /// 出票成功
            /// </summary>
            public const int Success = 0x0020;

            /// <summary>
            /// 出票失败
            /// </summary>
            public const int Failure = 0x0030;
        }

        /// <summary>
        /// 返奖状态
        /// </summary>
        public static class Awarding
        {
            /// <summary>
            /// 等待返奖
            /// </summary>
            public const int Waiting = 0x0100;

            /// <summary>
            /// 中奖
            /// </summary>
            public const int Winning = 0x0200;

            /// <summary>
            /// 未中奖
            /// </summary>
            public const int Loseing = 0x0300;
        }
    }
}
