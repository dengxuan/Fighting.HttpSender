using Baibaocp.Core;
using Baibaocp.LotteryDispatcher.Executers;
using Baibaocp.LotteryDispatcher.Models.Results;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatcher.Shanghai.Handlers
{
    public class ShanghaiAwardingExecuteHandler : ShanghaiExecuteHandler<AwardingExecuter, AwardingResult>
    {
        private readonly ILogger<ShanghaiAwardingExecuteHandler> _logger;

        public ShanghaiAwardingExecuteHandler(ShanghaiDispatcherOptions options, ILoggerFactory loggerFactory) : base(options, loggerFactory, "111")
        {
            _logger = loggerFactory.CreateLogger<ShanghaiAwardingExecuteHandler>();
        }

        protected override string MakeRequest(AwardingExecuter executer)
        {
            string[] values = new string[]
            {
                    string.Format("OrderID={0}", executer.OrderId)
            };

            return string.Join("_", values);
        }

        protected override AwardingResult MakeResult(XDocument document)
        {
            string Status = document.Element("ActionResult").Element("xCode").Value;
            if (Status.Equals("0"))
            {
                return new AwardingResult(OrderStatus.TicketWinning);
            }
            else if (Status.Equals("1"))
            {
                return new AwardingResult(OrderStatus.TicketDrawing);
            }
            else
            {
                // TODO: Log here and notice to admin
                _logger.LogWarning("Response message {0}", document.ToString(SaveOptions.DisableFormatting | SaveOptions.OmitDuplicateNamespaces));
            }
            return new AwardingResult(OrderStatus.TicketDrawing);
        }
    }
}
