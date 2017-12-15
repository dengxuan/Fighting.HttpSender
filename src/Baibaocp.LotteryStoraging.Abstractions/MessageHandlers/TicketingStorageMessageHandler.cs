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

namespace Baibaocp.LotteryStorager.MessageHandlers
{
    public class TicketingStorageMessageHandler : IMessageHandler<TicketedMessage>
    {

        private readonly StorageOptions _options;

        private readonly ICacheManager _cacheManager;

        private readonly IStringGenerator _stringGenerator;

        public TicketingStorageMessageHandler(StorageOptions options, ICacheManager cacheManager, IStringGenerator stringGenerator)
        {
            _options = options;
            _cacheManager = cacheManager;
            _stringGenerator = stringGenerator;
        }

        public async Task<bool> Handle(TicketedMessage message, CancellationToken token)
        {
            using (MySqlConnection connection = new MySqlConnection(_options.DefaultNameOrConnectionString))
            {
                await connection.ExecuteAsync("UPDATE `BbcpOrders` SET `LdpVenderId`=@LdpVenderId, `ChannelOrderId`=@LdpOrderId, `TicketOdds`=@TicketOdds, `Status` = @Status WHERE `Id`=@Id", new
                {
                    Id = message.LvpOrderId,
                    LdpVenderId = message.LdpVenderId,
                    LdpOrderId = message.LdpOrderId,
                    TicketOdds = message.TicketOdds,
                    Status = message.Status
                });
                if (message.Status == OrderStatus.TicketDrawing)
                {
                    /* 出票成功，上游扣款，增加出票金额 */
                    await connection.ExecuteAsync("UPDATE `BbcpChannels` SET `RestPreMoney` = `RestPreMoney` - @OrderAmount, `OutTicketMoney` = `OutTicketMoney` + @OrderAmount WHERE `Id` = @Id;", new
                    {
                        Id = message.LdpVenderId,
                        OrderAmount = message.Amount
                    });
                    await connection.ExecuteAsync("INSERT INTO `BbcpChannelAccountDetails`(`Id`,`ChannelId`,`LotteryId`,`OrderId`,`Amount`,`Status`,`CreationTime`)VALUES(@Id,@ChannelId,@LotteryId,@OrderId,@Amount,@Status,@CreationTime)", new
                    {
                        Id = _stringGenerator.Create(),
                        ChannelId = message.LdpVenderId,
                        LotteryId = message.LotteryId,
                        OrderId = message.LdpOrderId,
                        Amount = message.Amount,
                        Status = 3000,
                        CreationTime = DateTime.Now
                    });

                    /* 出票成功，下游增加出票金额 */
                    await connection.ExecuteAsync("UPDATE `BbcpChannels` SET `RestPreMoney` = `RestPreMoney` - @OrderAmount, `OutTicketMoney` = `OutTicketMoney` + @OrderAmount WHERE `Id` = @Id;", new
                    {
                        Id = message.LvpVenderId,
                        OrderAmount = message.Amount
                    });
                    await connection.ExecuteAsync("INSERT INTO `BbcpChannelAccountDetails`(`Id`,`ChannelId`,`LotteryId`,`OrderId`,`Amount`,`Status`,`CreationTime`)VALUES(@Id,@ChannelId,@LotteryId,@OrderId,@Amount,@Status,@CreationTime)", new
                    {
                        Id = _stringGenerator.Create(),
                        ChannelId = message.LvpVenderId,
                        LotteryId = message.LotteryId,
                        OrderId = message.LvpOrderId,
                        Amount = message.Amount,
                        Status = 2000,
                        CreationTime = DateTime.Now
                    });
                }
            }
            return true;
        }
    }
}
