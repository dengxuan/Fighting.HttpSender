using Baibaocp.Core;
using Baibaocp.LotteryDispatcher.Executers;
using Baibaocp.LotteryDispatcher.Models.Results;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatcher.Shanghai.Handlers
{
    public class ShanghaiTicketingExecuteHandler : ShanghaiExecuteHandler<TicketingExecuter, TicketingResult>
    {
        private readonly ILogger<ShanghaiTicketingExecuteHandler> _logger;

        public ShanghaiTicketingExecuteHandler(ShanghaiDispatcherOptions options, ILoggerFactory loggerFactory) : base(options, loggerFactory, "102")
        {
            _logger = loggerFactory.CreateLogger<ShanghaiTicketingExecuteHandler>();
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
            string Status = document.Element("ActionResult").Element("xCode").Value;
            if (Status.Equals("1"))
            {
                return new TicketingResult(OrderStatus.Ticketing.Success);
            }
            else if (Status.Equals("2002"))
            {
#if DEBUG
                return new TicketingResult(OrderStatus.Ticketing.Success);
#else
                return new TicketingResult(OrderStatus.Ticketing.Waiting);
#endif
            }
            else if (Status.Equals("2003"))
            {
                return new TicketingResult(OrderStatus.Ticketing.Failure);
            }
            else
            {
                // TODO: Log here and notice to admin
                _logger.LogWarning("Response message {0}", document.ToString(SaveOptions.DisableFormatting | SaveOptions.OmitDuplicateNamespaces));
            }
            return new TicketingResult(OrderStatus.Ticketing.Waiting);
        }
    }
}
