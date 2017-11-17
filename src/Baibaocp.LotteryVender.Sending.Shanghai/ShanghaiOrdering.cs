using System.Threading.Tasks;
using Baibaocp.LotteryVender.Sending.Abstractions;
using System.Net.Http;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Baibaocp.Extensions;

namespace Baibaocp.LotteryVender.Sending.Shanghai
{
    public class ShanghaiOrdering<TOrder> : IOrdering<TOrder> where TOrder : IOrder
    {
        private readonly HttpClient _httpClient;
        private readonly ShanghaiSenderOptions _options;

        private readonly string _command = "101";

        public ShanghaiOrdering(ShanghaiSenderOptions options)
        {
            _options = options;
            HttpClientHandler handler = new HttpClientHandler();
            handler.Proxy = new WebProxy("http://127.0.0.1:8888/");
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(options.Url);
        }

        public bool Send(TOrder order)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("LotteryId",""),
                new KeyValuePair<string, string>("PlayId",""),
                new KeyValuePair<string, string>("InvestCode",""),
                new KeyValuePair<string, string>("OrderId",""),
            });
            HttpResponseMessage responseMessage = _httpClient.PostAsync(_options.Url, content).GetAwaiter().GetResult();
            if (responseMessage.IsSuccessStatusCode)
            {
                Stream stream = responseMessage.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
                XDocument doc = XDocument.Load(stream);
                return true;
            }
            return false;
        }

        protected string Signature(DateTime timestamp, string value)
        {
            string text = string.Format("{0}{1}{2:yyyyMMddHHmm}{3}{4}", _options.VenderId, _command, timestamp, value, _options.SecretKey);
            return text.ToMd5();
        }

        public async Task<bool> SendAsync(TOrder order)
        {
            string[] values = new string[]
            {
                    string.Format("OrderID={0}", order.Id),
                    string.Format("LotID={0}", order.LotteryId),
                    string.Format("LotIssue={0}", order.IssueNumber),
                    string.Format("LotMoney={0}", order.InvestAmount/100),
                    string.Format("LotCode={0}", order.InvestCode),
                    string.Format("LotMulti={0}", order.InvestTimes),
                    string.Format("Attach={0}", ""),
                    string.Format("OneMoney={0}", order.InvestType == false ? "2":"3")
            };
            string value = string.Join("_", values);
            DateTime timestamp = DateTime.Now;
            string sign = Signature(timestamp, value);
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("wAgent", _options.VenderId),
                new KeyValuePair<string, string>("wAction", _command),
                new KeyValuePair<string, string>("wMsgID", timestamp.ToString("yyyyMMddHHmm")),
                new KeyValuePair<string, string>("wSign",sign.ToLower()),
                new KeyValuePair<string, string>("wParam",value),
            });
            try
            {
                HttpResponseMessage responseMessage = await _httpClient.PostAsync("lotsale/lot", content);
                if (responseMessage.IsSuccessStatusCode)
                {
                    string msg = await responseMessage.Content.ReadAsStringAsync();
                    XDocument xml = XDocument.Parse(msg);
                    string Status = xml.Element("ActionResult").Element("xCode").Value;
                    if (Status.Equals("0") || Status.Equals("1"))
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
    }
}
