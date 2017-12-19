using Baibaocp.Core;
using Baibaocp.Core.Messages;
using Baibaocp.LotteryDispatcher.Executers;
using Fighting.Extensions.Messaging.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatcher.Shanghai.Handlers
{
    public class ShanghaiAwardingExecuteHandler : ShanghaiExecuteHandler<AwardingExecuter>
    {
        private readonly IMessagePublisher _publisher;

        private readonly ILogger<ShanghaiAwardingExecuteHandler> _logger;

        public ShanghaiAwardingExecuteHandler(ShanghaiDispatcherOptions options, ILoggerFactory loggerFactory, IMessagePublisher publisher) : base(options, loggerFactory, "111")
        {
            _logger = loggerFactory.CreateLogger<ShanghaiAwardingExecuteHandler>();
            _publisher = publisher;
        }

        protected override string BuildRequest(AwardingExecuter executer)
        {
            string[] values = new string[]
            {
                    string.Format("OrderID={0}", executer.LdpOrderId)
            };

            return string.Join("_", values);
        }

        public override async Task<bool> HandleAsync(AwardingExecuter executer)
        {
            string xml = await Send(executer);
            XDocument document = XDocument.Parse(xml);

            string Status = document.Element("ActionResult").Element("xCode").Value;
            string value = document.Element("ActionResult").Element("xValue").Value;
            if (Status.Equals("0"))
            {
                string[] values = value.Split('_');
                AwardedMessage awardedMessage = new AwardedMessage
                {
                    LvpOrderId = executer.LdpOrderId,
                    LdpOrderId = executer.LdpOrderId,
                    LdpVenderId = executer.LdpVenderId,
                    Status = OrderStatus.TicketWinning,
                    Amount = Convert.ToInt32(values[2]) * 100
                };
                await _publisher.Publish(RoutingkeyConsts.Awards.Completed.Winning, awardedMessage, CancellationToken.None);
                return true;
            }
            // TODO: Log here and notice to admin
            _logger.LogWarning("Response message {0}", document.ToString(SaveOptions.DisableFormatting | SaveOptions.OmitDuplicateNamespaces));
            return false;
        }
    }
}
