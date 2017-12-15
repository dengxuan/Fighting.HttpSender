using System;
using System.Collections.Generic;
using System.Linq;

namespace Baibaocp.LotteryVender.Shanghai.Extensions
{
    public  class JcSaleIdDictionary
    {

        private static Dictionary<String, String> lotterydata = new Dictionary<String, String>();

        static JcSaleIdDictionary()
        {
            //串关计算
            lotterydata["500"] = "1*1";
            lotterydata["502"] = "2*1";
            lotterydata["503"] = "3*1";
            lotterydata["504"] = "4*1";
            lotterydata["505"] = "5*1";
            lotterydata["506"] = "6*1";
            lotterydata["507"] = "7*1";
            lotterydata["508"] = "8*1";

            lotterydata["526"] = "2*1"; //3*3
            lotterydata["527"] = "2*1,3*1"; //3*4

            lotterydata["539"] = "3*1"; //4*4
            lotterydata["540"] = "3*1,4*1"; //4*5
            lotterydata["528"] = "2*1"; //4*6
            lotterydata["529"] = "2*1,3*1,4*1"; //4*11

            lotterydata["544"] = "4*1"; //5*5
            lotterydata["545"] = "4*1,5*1"; //5*6
            lotterydata["530"] = "2*1"; //5*10
            lotterydata["541"] = "3*1,4*1,5*1"; //5*16
            lotterydata["531"] = "2*1,3*1";//5*20
            lotterydata["532"] = "2*1,3*1,4*1,5*1"; //5*26

            lotterydata["549"] = "5*1"; //6*6
            lotterydata["550"] = "5*1,6*1"; //6*7
            lotterydata["533"] = "2*1"; //6*15
            lotterydata["542"] = "3*1"; //6*20
            lotterydata["546"] = "4*1,5*1,6*1"; //6*22
            lotterydata["534"] = "2*1,3*1"; //6*35
            lotterydata["543"] = "3*1,4*1,5*1,6*1";//6*42
            lotterydata["535"] = "2*1,3*1,4*1";//6*50
            lotterydata["536"] = "2*1,3*1,4*1"; //6*57

            lotterydata["553"] = "6*1"; // 7串7
            lotterydata["554"] = "6*1,7*1"; // 7串8
            lotterydata["551"] = "5*1"; // 7串21
            lotterydata["547"] = "4*1"; // 7串35
            lotterydata["537"] = "2*1,3*1,4*1,5*1,6*1,7*1"; // 7串120

            lotterydata["556"] = "8*1"; // 8串8
            lotterydata["557"] = "7*1,8*1"; // 8串9
            lotterydata["555"] = "6*1"; // 8串28
            lotterydata["552"] = "5*1"; // 8串56
            lotterydata["548"] = "4*1"; // 8串70
            lotterydata["538"] = "2*1,3*1,4*1,5*1,6*1,7*1,8*1"; //8串247

            //串关转换(百宝转智赢)
            lotterydata["Z500"] = "单关";
            lotterydata["Z502"] = "2X1";
            lotterydata["Z503"] = "3X1";
            lotterydata["Z504"] = "4X1";
            lotterydata["Z505"] = "5X1";
            lotterydata["Z506"] = "6X1";
            lotterydata["Z507"] = "7X1";
            lotterydata["Z508"] = "8X1";

            lotterydata["Z526"] = "3X3";
            lotterydata["Z527"] = "3X4";

            lotterydata["Z539"] = "4X4"; //4*4
            lotterydata["Z540"] = "4X5"; //4*5
            lotterydata["Z528"] = "4X6"; //4*6
            lotterydata["Z529"] = "4X11"; //4*11

            lotterydata["Z544"] = "5X5"; //5*5
            lotterydata["Z545"] = "5X6"; //5*6
            lotterydata["Z530"] = "5X10"; //5*10
            lotterydata["Z541"] = "5X16"; //5*16
            lotterydata["Z531"] = "5X20";//5*20
            lotterydata["Z532"] = "5X26"; //5*26

            lotterydata["Z549"] = "6X6"; //6*6
            lotterydata["Z550"] = "6X7"; //6*7
            lotterydata["Z533"] = "6X15"; //6*15
            lotterydata["Z542"] = "6X20"; //6*20
            lotterydata["Z546"] = "6X22"; //6*22
            lotterydata["Z534"] = "6X35"; //6*35
            lotterydata["Z543"] = "6X42";//6*42
            lotterydata["Z535"] = "6X50";//6*50
            lotterydata["Z536"] = "6X57"; //6*57

            lotterydata["Z553"] = "7X7"; // 7串7
            lotterydata["Z554"] = "7X8"; // 7串8
            lotterydata["Z551"] = "7X21"; // 7串21
            lotterydata["Z547"] = "7X35"; // 7串35
            lotterydata["Z537"] = "7X120"; // 7串120

            lotterydata["Z556"] = "8X8"; // 8串8
            lotterydata["Z557"] = "8X9"; // 8串9
            lotterydata["Z555"] = "8X28"; // 8串28
            lotterydata["Z552"] = "8X56"; // 8串56
            lotterydata["Z548"] = "8X70"; // 8串70
            lotterydata["Z538"] = "8X247"; //8串247


            //串关转换(百宝转山东)
            lotterydata["S500"] = "1";
            lotterydata["S502"] = "2_1";
            lotterydata["S503"] = "3_1";
            lotterydata["S504"] = "4_1";
            lotterydata["S505"] = "5_1";
            lotterydata["S506"] = "6_1";
            lotterydata["S507"] = "7_1";
            lotterydata["S508"] = "8_1";

            lotterydata["S526"] = "3_3";
            lotterydata["S527"] = "3_4";

            lotterydata["S539"] = "4_4"; //4*4
            lotterydata["S540"] = "4_5"; //4*5
            lotterydata["S528"] = "4_6"; //4*6
            lotterydata["S529"] = "4_11"; //4*11

            lotterydata["S544"] = "5_5"; //5*5
            lotterydata["S545"] = "5_6"; //5*6
            lotterydata["S530"] = "5_10"; //5*10
            lotterydata["S541"] = "5_16"; //5*16
            lotterydata["S531"] = "5_20";//5*20
            lotterydata["S532"] = "5_26"; //5*26
        
            lotterydata["S549"] = "6_6"; //6*6
            lotterydata["S550"] = "6_7"; //6*7
            lotterydata["S533"] = "6_15"; //6*15
            lotterydata["S542"] = "6_20"; //6*20
            lotterydata["S546"] = "6_22"; //6*22
            lotterydata["S534"] = "6_35"; //6*35
            lotterydata["S543"] = "6_42";//6*42
            lotterydata["S535"] = "6_50";//6*50
            lotterydata["S536"] = "6_57"; //6*57

            lotterydata["S553"] = "7_7"; // 7串7
            lotterydata["S554"] = "7_8"; // 7串8
            lotterydata["S551"] = "7_21"; // 7串21
            lotterydata["S547"] = "7_35"; // 7串35
            lotterydata["S537"] = "7_120"; // 7串120
        
            lotterydata["S556"] = "8_8"; // 8串8
            lotterydata["S557"] = "8_9"; // 8串9
            lotterydata["S555"] = "8_28"; // 8串28
            lotterydata["S552"] = "8_56"; // 8串56
            lotterydata["S548"] = "8_70"; // 8串70
            lotterydata["S538"] = "8_247"; //8串247

            //百宝转量彩
            lotterydata["L500"] = "1*1";
            lotterydata["L502"] = "2*1";
            lotterydata["L503"] = "3*1";
            lotterydata["L504"] = "4*1";
            lotterydata["L505"] = "5*1";
            lotterydata["L506"] = "6*1";
            lotterydata["L507"] = "7*1";
            lotterydata["L508"] = "8*1";

            lotterydata["L526"] = "3*3";
            lotterydata["L527"] = "3*4";

            lotterydata["L539"] = "4*4"; //4*4
            lotterydata["L540"] = "4*5"; //4*5
            lotterydata["L528"] = "4*6"; //4*6
            lotterydata["L529"] = "4*11"; //4*11

            lotterydata["L544"] = "5*5"; //5*5
            lotterydata["L545"] = "5*6"; //5*6
            lotterydata["L530"] = "5*10"; //5*10
            lotterydata["L541"] = "5*16"; //5*16
            lotterydata["L531"] = "5*20";//5*20
            lotterydata["L532"] = "5*26"; //5*26

            lotterydata["L549"] = "6*6"; //6*6
            lotterydata["L550"] = "6*7"; //6*7
            lotterydata["L533"] = "6*15"; //6*15
            lotterydata["L542"] = "6*20"; //6*20
            lotterydata["L546"] = "6*22"; //6*22
            lotterydata["L534"] = "6*35"; //6*35
            lotterydata["L543"] = "6*42";//6*42
            lotterydata["L535"] = "6*50";//6*50
            lotterydata["L536"] = "6*57"; //6*57

            lotterydata["L553"] = "7*7"; // 7串7
            lotterydata["L554"] = "7*8"; // 7串8
            lotterydata["L551"] = "7*21"; // 7串21
            lotterydata["L547"] = "7*35"; // 7串35
            lotterydata["L537"] = "7*120"; // 7串120

            lotterydata["L556"] = "8*8"; // 8串8
            lotterydata["L557"] = "8*9"; // 8串9
            lotterydata["L555"] = "8*28"; // 8串28
            lotterydata["L552"] = "8*56"; // 8串56
            lotterydata["L548"] = "8*70"; // 8串70
            lotterydata["L538"] = "8*247"; //8串247


            //串关对应场次数
            lotterydata["N500"] = "1";
            lotterydata["N502"] = "2";
            lotterydata["N503"] = "3";
            lotterydata["N504"] = "4";
            lotterydata["N505"] = "5";
            lotterydata["N506"] = "6";
            lotterydata["N507"] = "7";
            lotterydata["N508"] = "8";
            lotterydata["N526"] = "3"; //3*3
            lotterydata["N527"] = "3"; //3*4
            lotterydata["N539"] = "4"; //4*4
            lotterydata["N540"] = "4"; //4*5
            lotterydata["N528"] = "4"; //4*6
            lotterydata["N529"] = "4"; //4*11
            lotterydata["N544"] = "5"; //5*5
            lotterydata["N545"] = "5"; //5*6
            lotterydata["N530"] = "5"; //5*10
            lotterydata["N541"] = "5"; //5*16
            lotterydata["N531"] = "5";//5*20
            lotterydata["N532"] = "5"; //5*26
            lotterydata["N549"] = "6"; //6*6
            lotterydata["N550"] = "6"; //6*7
            lotterydata["N533"] = "6"; //6*15
            lotterydata["N542"] = "6"; //6*20
            lotterydata["N546"] = "6"; //6*22
            lotterydata["N534"] = "6"; //6*35
            lotterydata["N543"] = "6";//6*42
            lotterydata["N535"] = "6";//6*50
            lotterydata["N536"] = "6"; //6*57
            lotterydata["N553"] = "7"; // 7串7
            lotterydata["N554"] = "7"; // 7串8
            lotterydata["N551"] = "7"; // 7串21
            lotterydata["N547"] = "7"; // 7串35
            lotterydata["N537"] = "7"; // 7串120

            /** 选择8场比赛 */
            lotterydata["N556"] = "8"; // 8串8
            lotterydata["N557"] = "8"; // 8串9
            lotterydata["N555"] = "8"; // 8串28
            lotterydata["N552"] = "8"; // 8串56
            lotterydata["N548"] = "8"; // 8串70
            lotterydata["N538"] = "8";

            
        }

        public static String ConvertLotteryId(string lotteryId)
        {
            if(lotterydata.Keys.Contains(lotteryId))
            {
                return lotterydata[lotteryId];
            }
            return lotteryId;
        }
    }
}
