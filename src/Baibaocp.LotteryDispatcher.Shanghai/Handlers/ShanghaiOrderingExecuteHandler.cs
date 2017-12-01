using Baibaocp.Core;
using Baibaocp.LotteryDispatcher.Executers;
using Baibaocp.LotteryDispatcher.Models.Results;
using Baibaocp.LotteryVender.Shanghai.Extensions;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatcher.Shanghai.Handlers
{
    public class ShanghaiOrderingExecuteHandler : ShanghaiExecuteHandler<OrderingExecuter, OrderingResult>
    {
        private readonly ILogger<ShanghaiOrderingExecuteHandler> _logger;

        public ShanghaiOrderingExecuteHandler(ShanghaiDispatcherOptions options, ILoggerFactory loggerFactory) : base(options, loggerFactory, "101")
        {
            _logger = loggerFactory.CreateLogger<ShanghaiOrderingExecuteHandler>();
        }

        protected override string MakeRequest(OrderingExecuter executer)
        {
            string[] values = new string[]
            {
                    string.Format("OrderID={0}", executer.OrderId),
                    string.Format("LotID={0}", executer.LotteryId.ToLottery()),
                    string.Format("LotIssue={0}", executer.IssueNumber),
                    string.Format("LotMoney={0}", executer.InvestAmount/100),
                    string.Format("LotCode={0}", executer.InvestCode.ToCastcode(executer.LotteryPlayId)),
                    string.Format("LotMulti={0}", executer.InvestTimes),
                    string.Format("Attach={0}", ""),
                    string.Format("OneMoney={0}", executer.InvestType == false ? "2":"3")
            };
            return string.Join("_", values);
        }

        protected override OrderingResult MakeResult(XDocument document)
        {
            string Status = document.Element("ActionResult").Element("xCode").Value;
            if (Status.Equals("0") || Status.Equals("1") || Status.Equals("1008"))
            {
                return new OrderingResult(OrderStatus.Ordering.Success);
            }
            else
            {
                // TODO: Log here and notice to admin
                _logger.LogWarning("Response message {0}", document.ToString(SaveOptions.DisableFormatting | SaveOptions.OmitDuplicateNamespaces));
            }
            return new OrderingResult(OrderStatus.Ordering.Waiting);
        }
    }
}
