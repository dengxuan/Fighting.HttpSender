using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.Models;
using Baibaocp.LotteryDispatcher.Shanghai;
using Fighting.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatcher.ShanghaiTicketing.Handlers
{
    public class OrderingCommandHandler : IExecuteHandler<TicketingExecuter>
    {
        private const string COMMAND = "102";

        private readonly HttpClient _httpClient;
        private readonly ShanghaiDispatcherOptions _options;

        public string LvpVenderId { get; }

        public OrderingCommandHandler(ShanghaiDispatcherOptions options)
        {
            _options = options;
            LvpVenderId = options.VenderId;
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.Deflate
            };
            //handler.Proxy = new WebProxy("http://127.0.0.1:8888/");
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(options.Url)
            };
        }


        protected string Signature(DateTime timestamp, string value)
        {
            string text = string.Format("{0}{1}{2:yyyyMMddHHmm}{3}{4}", LvpVenderId, COMMAND, timestamp, value, _options.SecretKey);
            return text.ToMd5();
        }

        public async Task<ExecuteResult> HandleAsync(TicketingExecuter command)
        {
            string[] values = new string[]
            {
                    string.Format("OrderID={0}", command.OrderId)
            };
            string value = string.Join("_", values);
            DateTime timestamp = DateTime.Now;
            string sign = Signature(timestamp, value);
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("wAgent", LvpVenderId),
                new KeyValuePair<string, string>("wAction",COMMAND),
                new KeyValuePair<string, string>("wMsgID", timestamp.ToString("yyyyMMddHHmm")),
                new KeyValuePair<string, string>("wSign",sign.ToLower()),
                new KeyValuePair<string, string>("wParam",value),
            });
            HttpResponseMessage responseMessage = await _httpClient.PostAsync("lotsale/lot", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                byte[] bytes = await responseMessage.Content.ReadAsByteArrayAsync();
                string msg = Encoding.Default.GetString(bytes);
                XDocument xml = XDocument.Parse(msg);
                string Status = xml.Element("ActionResult").Element("xCode").Value;
                if (Status.Equals("0") || Status.Equals("1"))
                {
                    return new ExecuteResult() { VenderId = _options.VenderId };
                }
                return new ExecuteResult(false);
            }
            return new ExecuteResult(new ExecuteError());
        }
    }
}
