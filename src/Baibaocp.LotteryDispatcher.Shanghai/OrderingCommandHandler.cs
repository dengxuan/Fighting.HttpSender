using Baibaocp.Extensions;
using Baibaocp.LotteryCommand.Abstractions;
using Baibaocp.LotteryCommand.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Baibaocp.LotteryVender.Sending.Shanghai
{
    public class OrderingCommandHandler : ICommandHandlerAsync<OrderingCommand>
    {
        private readonly HttpClient _httpClient;
        private readonly ShanghaiSenderOptions _options;
        private readonly string _commandId = "101";

        public OrderingCommandHandler(ShanghaiSenderOptions options)
        {
            _options = options;
            HttpClientHandler handler = new HttpClientHandler();
            //handler.Proxy = new WebProxy("http://127.0.0.1:8888/");
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(options.Url)
            };
        }


        protected string Signature(DateTime timestamp, string value)
        {
            string text = string.Format("{0}{1}{2:yyyyMMddHHmm}{3}{4}", _options.VenderId, _commandId, timestamp, value, _options.SecretKey);
            return text.ToMd5();
        }

        public async Task<ExecuteResult> HandleAsync(OrderingCommand command)
        {
            string[] values = new string[]
            {
                    string.Format("OrderID={0}", command.OrderId),
                    string.Format("LotID={0}", command.LotteryId),
                    string.Format("LotIssue={0}", command.IssueNumber),
                    string.Format("LotMoney={0}", command.InvestAmount/100),
                    string.Format("LotCode={0}", command.InvestCode),
                    string.Format("LotMulti={0}", command.InvestTimes),
                    string.Format("Attach={0}", ""),
                    string.Format("OneMoney={0}", command.InvestType == false ? "2":"3")
            };
            string value = string.Join("_", values);
            DateTime timestamp = DateTime.Now;
            string sign = Signature(timestamp, value);
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("wAgent", _options.VenderId),
                new KeyValuePair<string, string>("wAction",_commandId),
                new KeyValuePair<string, string>("wMsgID", timestamp.ToString("yyyyMMddHHmm")),
                new KeyValuePair<string, string>("wSign",sign.ToLower()),
                new KeyValuePair<string, string>("wParam",value),
            });
            HttpResponseMessage responseMessage = await _httpClient.PostAsync("lotsale/lot", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                string msg = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                XDocument xml = XDocument.Parse(msg);
                string Status = xml.Element("ActionResult").Element("xCode").Value;
                if (Status.Equals("0") || Status.Equals("1"))
                {
                    return new ExecuteResult();
                }
                return new ExecuteResult(false);
            }
            return new ExecuteResult(new ExecuteError());
        }
    }
}
