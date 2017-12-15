using Baibaocp.LotteryVender.Shanghai.Extensions;
using Fighting.Extensions.Caching.Abstractions;
using System;
using System.Xml.Linq;

public class JcOdds
{
    private readonly ICache cacher;


    public JcOdds(ICacheManager cacheManager)
    {
        cacher = cacheManager.GetCache("RP");
    }

    //protected string Odds(int lotteryid, XElement info, int playtypeid)
    //{
    //    string newcode = string.Empty;
    //    foreach (XElement item in info.Elements("match"))
    //    {
    //        string oldeventid = item.Attribute("id").Value;
    //        int year = Convert.ToInt32("20" + oldeventid.Substring(0, 2));
    //        int month = Convert.ToInt32(oldeventid.Substring(2, 2));
    //        int day = Convert.ToInt32(oldeventid.Substring(4, 2));
    //        string playid = oldeventid.Substring(6, 3);
    //        DateTime dt = new DateTime(year, month, day);
    //        int week = (int)dt.DayOfWeek;
    //        if (week == 0)
    //        {
    //            week = 7;
    //        }
    //        string neweventid = string.Empty;
    //        int lotid = 0;
    //        if (lotteryid == 20205)
    //        {
    //            lotid = (int)item.Attribute("playid").Value.ToBaibaoLottery();
    //            neweventid = dt.ToString("yyyyMMdd") + week.ToString() + playid + "-" + lotid.ToString();
    //        }
    //        else
    //        {
    //            neweventid = dt.ToString("yyyyMMdd") + week.ToString() + playid;
    //            lotid = (int)lotteryid;
    //        }
    //        string eventid = dt.ToString("yyyyMMdd") + week.ToString() + playid;
    //        var @event = cacher.GetAsync(eventid, (cackeKey) => { return null; });
    //        string newodds = string.Empty;


    //        if (lotid == 20201)
    //        {
    //            string oldodds = item.Value;
    //            if (playtypeid == 500)
    //            {
    //                newodds = newodds + neweventid + "@0|" + oldodds.Replace("*", "1.00").Replace("=", "*").Replace("|", "#") + "#";
    //            }
    //            else
    //            {
    //                newodds = newodds + neweventid + "@0|" + oldodds.Replace("=", "*").Replace("|", "#") + "#";
    //            }
    //        }
    //        if (lotid == 20206)
    //        {
    //            string oldodds = item.Value;
    //            string rq = string.Empty;
    //            if (item.Attribute("rq") != null)
    //            {
    //                rq = item.Attribute("rq").Value;
    //            }
    //            else
    //            {
    //                LpOdds odds = this.oddstorer.Select(this.Selector);
    //                LpZcOdds zcodds = odds.zcodds;
    //                rq = zcodds.rqspf.let;
    //            }
    //            //string rq = item.Attribute("rq").Value;

    //            if (playtypeid == 500)
    //            {
    //                newodds = newodds + neweventid + "@" + rq + "|" + oldodds.Replace("*", "1.00").Replace("=", "*").Replace("|", "#") + "#";
    //            }
    //            else
    //            {
    //                newodds = newodds + neweventid + "@" + rq + "|" + oldodds.Replace("=", "*").Replace("|", "#") + "#";
    //            }
    //        }
    //        if (lotid == 20202)
    //        {
    //            string oldodds = item.Value;
    //            if (playtypeid == 500)
    //            {
    //                newodds = newodds + neweventid + "@0|" + oldodds.Replace("*", "1.00").Replace(":", "").Replace("=", "*").Replace("|", "#") + "#";
    //            }
    //            else
    //            {
    //                newodds = newodds + neweventid + "@0|" + oldodds.Replace(":", "").Replace("=", "*").Replace("|", "#") + "#";
    //            }
    //        }
    //        if (lotid == Convert.ToInt32(LotteryTypes.JcZjq))
    //        {
    //            string oldodds = item.Value;
    //            if (playtypeid == 500)
    //            {
    //                newodds = newodds + neweventid + "@0|" + oldodds.Replace("*", "1.00").Replace("=", "*").Replace("|", "#") + "#";
    //            }
    //            else
    //            {
    //                newodds = newodds + neweventid + "@0|" + oldodds.Replace("=", "*").Replace("|", "#") + "#";
    //            }
    //        }
    //        if (lotid == Convert.ToInt32(LotteryTypes.JcBqc))
    //        {
    //            string oldodds = item.Value;
    //            if (playtypeid == 500)
    //            {
    //                newodds = newodds + neweventid + "@0|" + oldodds.Replace("*", "1.00").Replace("-", "").Replace("=", "*").Replace("|", "#") + "#";
    //            }
    //            else
    //            {
    //                newodds = newodds + neweventid + "@0|" + oldodds.Replace("-", "").Replace("=", "*").Replace("|", "#") + "#";
    //            }
    //        }
    //        newcode = newcode + newodds + "^";
    //    }
    //    newcode = newcode.TrimEnd('^');
    //    return newcode;
    //}


}