using Baibaocp.Core;
using Baibaocp.Core.Messages;
using Baibaocp.LotteryVender.Core.Entities;
using Dapper;
using Fighting.Extensions.Caching.Abstractions;
using Fighting.Extensions.Messaging.Abstractions;
using Fighting.Storaging;
using Fighting.Storaging.Abstractions;
using Fighting.Storaging.Repositories.Abstractions;
using Pomelo.Data.MySql;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Baibaocp.LotteryStoraging.Abstractions.MessageHandlers
{
    public class AwardingStorageMessageHandler : IMessageHandler<AwardedMessage>
    {
        private readonly StorageOptions _options;

        private readonly ICacheManager _cacheManager;

        private readonly IStringGenerator _stringGenerator;

        public AwardingStorageMessageHandler(StorageOptions options, ICacheManager cacheManager, IStringGenerator stringGenerator)
        {
            _options = options;
            _cacheManager = cacheManager;
            _stringGenerator = stringGenerator;
        }

        public async Task<bool> Handle(AwardedMessage message, CancellationToken token)
        {
            using (MySqlConnection connection = new MySqlConnection(_options.DefaultNameOrConnectionString))
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    if (message.Status == OrderStatus.TicketWinning)
                    {

                        await connection.ExecuteAsync("UPDATE `BbcpOrders` SET `BonusAmount`=@BonusAmount, `AfterTaxBonusAmount`=@AftertaxBonusAmount, `Status` = `Status` | @Status WHERE `Id`=@Id", new
                        {
                            Id = message.LvpOrderId,
                            BonusAmount = message.Amount,
                            AftertaxBonusAmount = message.AftertaxAmount,
                            Status = OrderStatus.TicketWinning
                        });

                        // ldp
                        await connection.ExecuteAsync("UPDATE `BbcpChannels` SET `RestPreMoney` = `RestPreMoney` + @Amount, `RewardMoney` = `RewardMoney` + @Amount WHERE `Id` = @Id;", new
                        {
                            Id = message.LdpVenderId,
                            Amount = message.Amount
                        });
                        await connection.ExecuteAsync("INSERT INTO `BbcpChannelAccountDetails`(`Id`,`ChannelId`,`LotteryId`,`OrderId`,`Amount`,`Status`,`CreationTime`)VALUES(@Id,@ChannelId,@LotteryId,@OrderId,@Amount,@Status,@CreationTime)", new
                        {
                            Id = _stringGenerator.Create(),
                            ChannelId = message.LdpVenderId,
                            LotteryId = message.LotteryId,
                            OrderId = message.LvpOrderId,
                            Amount = message.Amount,
                            Status = OrderStatus.TicketWinning,
                            CreationTime = DateTime.Now
                        });

                        //lvp
                        await connection.ExecuteAsync("UPDATE `BbcpChannels` SET `RestPreMoney` = `RestPreMoney` + @Amount, `RewardMoney` = `RewardMoney` + @Amount WHERE `Id` = @Id;", new
                        {
                            Id = message.LvpVenderId,
                            Amount = message.Amount
                        });
                        await connection.ExecuteAsync("INSERT INTO `BbcpChannelAccountDetails`(`Id`,`ChannelId`,`LotteryId`,`OrderId`,`Amount`,`Status`,`CreationTime`)VALUES(@Id,@ChannelId,@LotteryId,@OrderId,@Amount,@Status,@CreationTime)", new
                        {
                            Id = _stringGenerator.Create(),
                            ChannelId = message.LvpVenderId,
                            LotteryId = message.LotteryId,
                            OrderId = message.LvpOrderId,
                            Amount = message.Amount,
                            Status = OrderStatus.TicketWinning,
                            CreationTime = DateTime.Now
                        });
                    }
                    else if (message.Status == OrderStatus.TicketLosing)
                    {
                        await connection.ExecuteAsync("UPDATE `BbcpOrders` SET `LdpVenderId`=@LdpVenderId, `Status` = `Status` | @Status WHERE `Id`=@Id", new
                        {
                            Id = message.LdpVenderId,
                            LdpVenderId = message.LdpVenderId,
                            Status = OrderStatus.TicketLosing
                        });
                    }
                    transaction.Complete();
                }
            }
            //ICache cacher = _cacheManager.GetCache("LotteryVender.Orders");
            //await cacher.SetAsync(order.Id, Task.FromResult(order));
            return true;
        }
    }
}
