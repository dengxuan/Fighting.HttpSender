﻿using Baibaocp.Core;
using Baibaocp.LotteryDispatcher.Executers;
using Baibaocp.LotteryDispatcher.Models.Results;
using Dapper;
using Fighting.Storaging;
using Microsoft.Extensions.Logging;
using Pomelo.Data.MySql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatcher.Shanghai.Handlers
{
    public class ShanghaiTicketingExecuteHandler : ShanghaiExecuteHandler<TicketingExecuter, TicketingResult>
    {
        private readonly ILogger<ShanghaiTicketingExecuteHandler> _logger;
        private readonly StorageOptions _storageOptions;

        public ShanghaiTicketingExecuteHandler(ShanghaiDispatcherOptions options, StorageOptions storageOptions, ILoggerFactory loggerFactory) : base(options, loggerFactory, "102")
        {
            _logger = loggerFactory.CreateLogger<ShanghaiTicketingExecuteHandler>();
            _storageOptions = storageOptions;
        }

        /// <summary>  
        /// zlib.net 解压函数
        /// </summary>  
        /// <param name="strSource">带解压数据源</param>  
        /// <returns>解压后的数据</returns>  
        public static string DeflateDecompress(string strSource)
        {
            byte[] Buffer = Convert.FromBase64String(strSource); // 解base64  
            using (MemoryStream intms = new MemoryStream(Buffer))
            {
                using (zlib.ZInputStream inZStream = new zlib.ZInputStream(intms))
                {
                    int size = short.MaxValue;
                    byte[] buffer = new byte[size];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        while ((size = inZStream.read(buffer, 0, size)) != -1)
                        {
                            ms.Write(buffer, 0, size);
                        }
                        inZStream.Close();
                        return Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);
                    }
                }
            }
        }
        protected override string MakeRequest(TicketingExecuter executer)
        {
            string[] values = new string[]
            {
                    string.Format("OrderID={0}", executer.OrderId)
            };
            return string.Join("_", values);
        }

        protected override TicketingResult MakeResult(XDocument document)
        {
            _logger.LogTrace(document.ToString());

            string Status = document.Element("ActionResult").Element("xCode").Value;
            if (Status.Equals("1"))
            {
                string odds = document.Element("ActionResult").Element("xValue").Value.Split('_')[3];
                string xml = DeflateDecompress(odds);

                return new TicketingResult(OrderStatus.TicketDrawing)
                {
                    TicketOdds = GetOdds(xml)
                };
            }
            else if (Status.Equals("2002"))
            {
#if DEBUG
                return new TicketingResult(OrderStatus.Ticketing);
#else
                return new TicketingResult(OrderStatus.Ticketing);
#endif
            }
            else if (Status.Equals("2003"))
            {
                return new TicketingResult(OrderStatus.TicketFailed);
            }
            else
            {
                // TODO: Log here and notice to admin
                _logger.LogWarning("Response message {0}", document.ToString(SaveOptions.DisableFormatting | SaveOptions.OmitDuplicateNamespaces));
            }
            return new TicketingResult(OrderStatus.Ticketing);
        }

        protected string GetOdds(string xml)
        {
            using (MySqlConnection connection = new MySqlConnection(_storageOptions.DefaultNameOrConnectionString))
            {
                XElement element = XElement.Parse(xml);
                IEnumerable<XElement> bills = element.Elements("bill");
                StringBuilder sb = new StringBuilder();
                foreach (var bill in bills)
                {
                    IEnumerable<XElement> matches = bill.Elements("match");
                    foreach (var match in matches)
                    {
                        string attr = $"20{match.Attribute("id").Value}";
                        DateTime date = DateTime.ParseExact(attr.Substring(0, 8), "yyyyMMdd", CultureInfo.CurrentCulture);
                        string @event = attr.Substring(8);
                        string id = $"{date.ToString("yyyyMMdd")}{(date.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)date.DayOfWeek)}{@event}";
                        var rateCount = connection.ExecuteScalar("SELECT `RqspfRateCount` FROM `BbcpZcEvents` WHERE `Id` = @Id", new { Id = id });
                        string odds = match.Value.Replace('=', '*').Replace(',', '#');
                        sb.Append($"{id}@{rateCount}|{odds}#^");
                    }
                }
                return sb.ToString();

            }
        }
    }
}
