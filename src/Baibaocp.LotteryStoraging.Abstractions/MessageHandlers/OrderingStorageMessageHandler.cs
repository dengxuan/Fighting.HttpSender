using Baibaocp.Core;
using Baibaocp.Core.Messages;
using Dapper;
using Fighting.Extensions.Caching.Abstractions;
using Fighting.Extensions.Messaging.Abstractions;
using Fighting.Storaging;
using Fighting.Storaging.Abstractions;
using Pomelo.Data.MySql;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Baibaocp.LotteryStorager.MessageHandlers
{
    public class OrderingStorageMessageHandler : IMessageHandler<OrderingMessage>
    {

        private readonly StorageOptions _options;

        private readonly ICacheManager _cacheManager;

        private readonly IMessagePublisher _publisher;

        private readonly IStringGenerator _stringGenerator;


        public OrderingStorageMessageHandler(StorageOptions options, ICacheManager cacheManager, IMessagePublisher publisher, IStringGenerator stringGenerator)
        {
            _options = options;
            _cacheManager = cacheManager;
            _publisher = publisher;
            _stringGenerator = stringGenerator;
        }

        public async Task<bool> Handle(OrderingMessage message, CancellationToken token)
        {
            message.Status = OrderStatus.Succeed;
            message.CreationTime = DateTime.Now;
            using (MySqlConnection connection = new MySqlConnection(_options.DefaultNameOrConnectionString))
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    await connection.ExecuteAsync(@"INSERT INTO `Baibaocp`.`BbcpOrders`(`Id`,`LotteryBuyerId`,`LvpUserId`,`LvpVenderId`,`LotteryId`,`LotteryPlayId`,`IssueNumber`,`InvestCode`,`InvestType`,`InvestCount`,`InvestTimes`,`InvestAmount`,`Status`,`CreationTime`)VALUES(@Id,@LotteryBuyerId,@LvpUserId,@LvpVenderId,@LotteryId,@LotteryPlayId,@IssueNumber,@InvestCode,@InvestType,@InvestCount,@InvestTimes,@InvestAmount,@Status,@CreationTime);", new
                    {
                        Id = message.LvpOrderId,
                        LotteryBuyerId = 619,
                        LvpUserId = message.LvpUserId,
                        LvpVenderId = message.LvpVenderId,
                        LotteryId = message.LotteryId,
                        LotteryPlayId = message.LotteryPlayId,
                        IssueNumber = message.IssueNumber,
                        InvestCode = message.InvestCode,
                        InvestType = message.InvestType,
                        InvestCount = message.InvestCount,
                        InvestTimes = message.InvestTimes,
                        InvestAmount = message.InvestAmount,
                        Status = message.Status,
                        CreationTime = message.CreationTime
                    });
                    await _publisher.Publish($"{RoutingkeyConsts.Orders.Storaged}.{message.LotteryId}.{message.LotteryPlayId}", message, token);
                    transaction.Complete();
                }
            }
            return true;
        }
    }
}
