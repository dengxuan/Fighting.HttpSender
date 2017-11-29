using System;

namespace Baibaocp.LotteryVender.Shanghai.Extensions
{
    public static class LotteryExtensions
    {
        public static string ToLottery(this int lotteryType)
        {
            switch (lotteryType)
            {
                case 1: return "51";
                case 31: return "33";
                case 40: return "35";
                case 2: return "23529";
                case 5: return "10022";
                case 20201:
                case 20202:
                case 20203:
                case 20204:
                case 20205:
                case 20206:
                    return "42";
                case 20401:
                case 20402:
                case 20403:
                case 20404:
                case 20405:
                    return "43";
                default: throw new ArgumentException("LotteryType Not Support: {0}", lotteryType.ToString());
            }
        }


        public static string ToShanghaiCodeLottery(this int lotteryType)
        {
            switch (lotteryType)
            {
                case 20201:
                    return "SPF";
                case 20202:
                    return "CBF";
                case 20204:
                    return "BQC";
                case 20205:
                    return "HH";
                case 20206:
                    return "RQSPF";
                case 20203:
                    return "JQS";
                case 20404:
                    return "DXF";
                case 20405:
                    return "HH";
                case 20402:
                    return "RFSF";
                case 20401:
                    return "SF";
                case 20403:
                    return "SFC";
                default: throw new ArgumentException("LotteryType Not Support: {0}", lotteryType.ToString());
            }
        }


        public static string ToPlay(this int playType)
        {
            switch (playType)
            {
                case 10021071:
                case 10022071:
                case 10023071:
                    return "";
                case 10041031:
                case 10042031: return "1";
                case 10041032:
                case 10042032:
                case 10041062:
                case 10042062: return "6";
                default: throw new ArgumentException("PlayType Not Support: {0}", playType.ToString());
            }
        }

        public static int ToBaibaoLottery(this string lotteryType)
        {
            switch (lotteryType)
            {
                case "SPF":
                case "20201":
                    return 20201;
                case "CBF":
                case "20202":
                    return 20202;
                case "JQS":
                case "20203":
                    return 20203;
                case "BQC":
                case "20204":
                    return 20204;
                case "HH":
                case "20205":
                    return 20205;
                case "RQSPF":
                case "20206":
                    return 20206;
                default: throw new ArgumentException("LotteryType Not Support: {0}", lotteryType.ToString());
            }
        }

        public static int ToBaibaoLcLottery(this string lotteryType)
        {
            switch (lotteryType)
            {
                case "SF":
                case "20401":
                    return 20401;
                case "RFSF":
                case "20402":
                    return 20402;
                case "SFC":
                case "20403":
                    return 20403;
                case "DXF":
                case "20404":
                    return 20404;
                case "HH":
                case "20405":
                    return 20405;
                default: throw new ArgumentException("LotteryType Not Support: {0}", lotteryType.ToString());
            }
        }
    }
}
