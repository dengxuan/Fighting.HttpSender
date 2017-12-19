using Baibaocp.Core;
using Baibaocp.LotteryDispatcher.Executers;
using Baibaocp.LotteryVender.Shanghai.Extensions;
using Fighting.Extensions.Messaging.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatcher.Shanghai.Handlers
{
    public class ShanghaiOrderingExecuteHandler : ShanghaiExecuteHandler<OrderingExecuter>
    {
        private readonly IMessagePublisher _publisher;

        private readonly ILogger<ShanghaiOrderingExecuteHandler> _logger;

        public ShanghaiOrderingExecuteHandler(ShanghaiDispatcherOptions options, ILoggerFactory loggerFactory, IMessagePublisher publisher) : base(options, loggerFactory, "101")
        {
            _logger = loggerFactory.CreateLogger<ShanghaiOrderingExecuteHandler>();
            _publisher = publisher;
        }

        protected override string BuildRequest(OrderingExecuter executer)
        {
            var order = executer.LvpOrders.Select(selector => new { LotteryId = selector.LotteryId, LotteryPlayId = selector.LotteryPlayId, IssueNumber = selector.IssueNumber, InvestTimes = selector.InvestTimes, InvestType = selector.InvestType }).FirstOrDefault();
            var amount = executer.LvpOrders.Sum(selector => selector.InvestAmount);
            string codes = string.Join(";", executer.LvpOrders.Select(selector => ShanghaiJcCode.ReturnShanghaiCode(selector.InvestCode, selector.LotteryId, selector.LotteryPlayId)));
            string[] values = new string[]
            {
                string.Format("OrderID={0}", executer.LdpOrderId),
                string.Format("LotID={0}", order.LotteryId.ToLottery()),
                string.Format("LotIssue={0}", order.IssueNumber),
                string.Format("LotMoney={0}", amount/100),
                string.Format("LotCode={0}", codes),
                string.Format("LotMulti={0}", order.InvestTimes),
                string.Format("Attach={0}", ""),
                string.Format("OneMoney={0}", order.InvestType ? "3":"2")
            };
            return string.Join("_", values);
        }

        public override async Task<bool> HandleAsync(OrderingExecuter executer)
        {
            try
            {
                string xml = await Send(executer);
                XDocument document = XDocument.Parse(xml);

                string Status = document.Element("ActionResult").Element("xCode").Value;
                if (Status.Equals("0") || Status.Equals("1") || Status.Equals("1008"))
                {
                    await _publisher.Publish(RoutingkeyConsts.Tickets.Completed.Success, new
                    {
                        LdpOrderId = executer.LdpOrderId,
                        LdpVenderId = executer.LdpVenderId,
                        LvpOrders = executer.LvpOrders,
                        Status = OrderStatus.TicketNotRecv
                    }, CancellationToken.None);
                }
                else if (Status.Equals("1003") || Status.Equals("1004"))
                {
                    // TODO: Log here and notice to admin
                    _logger.LogWarning("Response message {0}", document.ToString(SaveOptions.DisableFormatting | SaveOptions.OmitDuplicateNamespaces));

                    await _publisher.Publish(RoutingkeyConsts.Orders.Completed.Failure, new
                    {
                        LdpOrderId = executer.LdpOrderId,
                        LdpVenderId = executer.LdpVenderId,
                        LvpOrders = executer.LvpOrders,
                        Status = OrderStatus.TicketNotRecv
                    }, CancellationToken.None);

                }
                else if (Status.Equals("1016"))
                {
                    // TODO: Log here and notice to admin
                    _logger.LogWarning("Response message {0}", document.ToString(SaveOptions.DisableFormatting | SaveOptions.OmitDuplicateNamespaces));

                    await _publisher.Publish(RoutingkeyConsts.Orders.Completed.Failure, new
                    {
                        LdpOrderId = executer.LdpOrderId,
                        LdpVenderId = executer.LdpVenderId,
                        LvpOrders = executer.LvpOrders,
                        Status = OrderStatus.TicketNotSend
                    }, CancellationToken.None);

                }
                _logger.LogError("Response message {0}", document.ToString(SaveOptions.DisableFormatting | SaveOptions.OmitDuplicateNamespaces));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Request Exception:", ex);

                await _publisher.Publish(RoutingkeyConsts.Orders.Completed.Failure, new
                {
                    LdpOrderId = executer.LdpOrderId,
                    LdpVenderId = executer.LdpVenderId,
                    LvpOrders = executer.LvpOrders,
                    Status = OrderStatus.TicketNotRecv
                }, CancellationToken.None);
            }
            return false;
        }
    }
}
