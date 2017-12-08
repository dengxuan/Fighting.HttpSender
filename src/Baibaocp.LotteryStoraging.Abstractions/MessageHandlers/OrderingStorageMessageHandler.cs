using Baibaocp.Core.Messages;
using Baibaocp.LotteryVender.Core.Entities;
using Fighting.Extensions.Caching.Abstractions;
using Fighting.Extensions.Messaging.Abstractions;
using Fighting.Storaging.Repositories.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryStorager.Abstractions.MessageHandlers
{
    internal class OrderingStorageMessageHandler : IMessageHandler<OrderingMessage>
    {
        private readonly ICacheManager _cacheManager;

        private readonly IRepository<LotteryVenderEntities, string> _venderRepository;

        private readonly IRepository<LotteryVenderOrderEntity, string> _venderOrderRepository;

        private readonly IRepository<LotteryVenderAccountDetailEntity, string> _venderAccountDetailRepository;


        public OrderingStorageMessageHandler(ICacheManager cacheManager, IRepository<LotteryVenderEntities, string> venderRepository, IRepository<LotteryVenderOrderEntity, string> venderOrderRepository, IRepository<LotteryVenderAccountDetailEntity, string> venderAccountDetailRepository)
        {
            _cacheManager = cacheManager;
            _venderRepository = venderRepository;
            _venderOrderRepository = venderOrderRepository;
            _venderAccountDetailRepository = venderAccountDetailRepository;
        }

        public async Task<bool> Handle(OrderingMessage message, CancellationToken token)
        {
            LotteryVenderEntities lotteryVender = await _venderRepository.GetAsync(message.LvpVenderId);
            LotteryVenderOrderEntity order = new LotteryVenderOrderEntity
            {
                Id = message.OrderId,
                LvpOrderId = message.OrderId,
                LvpVenderId = message.LvpVenderId,
                LvpUserId = message.LvpUserId,
                LotteryBuyerId = 0,
                LotteryId = message.LotteryId,
                LotteryPlayId = message.LotteryPlayId,
                InvestCode = message.InvestCode,
                InvestAmount = message.InvestAmount,
                InvestCount = message.InvestCount,
                InvestTimes = message.InvestTimes,
                InvestType = message.InvestType,
                IssueNumber = message.IssueNumber,
                Status = message.Status,
                CreationTime = message.CreationTime
            };
            lotteryVender.Balance = lotteryVender.Balance - message.InvestAmount;
            lotteryVender.TicketAmount = lotteryVender.TicketAmount + message.InvestAmount;
            await _venderRepository.UpdateAsync(lotteryVender);
            await _venderAccountDetailRepository.InsertAsync(new LotteryVenderAccountDetailEntity { Id = Guid.NewGuid().ToString("N"), VenderId =message.LvpVenderId, OrderId = message.OrderId, LotteryId = message.LotteryId, Amount = message.InvestAmount, Status = message.Status, CretionTime = DateTime.Now });
            await _venderAccountDetailRepository.InsertAsync(new LotteryVenderAccountDetailEntity { Id = Guid.NewGuid().ToString("N"), VenderId = message.LdpVenderId, OrderId = message.LdpVenderId, LotteryId = message.LotteryId, Amount = message.InvestAmount, Status = message.Status, CretionTime = DateTime.Now });
            await _venderOrderRepository.InsertAsync(order);
            ICache cacher = _cacheManager.GetCache("LotteryVender.Orders");
            await cacher.SetAsync(message.OrderId, Task.FromResult(order));
            return true;
        }
    }
}
