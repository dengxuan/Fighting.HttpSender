using Baibaocp.Core;
using Baibaocp.Core.Messages;
using Baibaocp.LotteryVender.Core.Entities;
using Fighting.Extensions.Caching.Abstractions;
using Fighting.Extensions.Messaging.Abstractions;
using Fighting.Storaging.Repositories.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryStoraging.Abstractions.MessageHandlers
{
    internal class AwardingStorageMessageHandler : IMessageHandler<AwardingMessage>
    {
        private readonly ICacheManager _cacheManager;

        private readonly IRepository<LotteryVenderEntities, string> _venderRepository;

        private readonly IRepository<LotteryVenderOrderEntity, string> _venderOrderRepository;

        private readonly IRepository<LotteryVenderAccountDetailEntity, string> _venderAccountDetailRepository;

        public AwardingStorageMessageHandler(ICacheManager cacheManager, IRepository<LotteryVenderEntities, string> venderRepository, IRepository<LotteryVenderOrderEntity, string> venderOrderRepository, IRepository<LotteryVenderAccountDetailEntity, string> venderAccountDetailRepository)
        {
            _cacheManager = cacheManager;
            _venderRepository = venderRepository;
            _venderOrderRepository = venderOrderRepository;
            _venderAccountDetailRepository = venderAccountDetailRepository;
        }

        public async Task<bool> Handle(AwardingMessage message, CancellationToken token)
        {
            LotteryVenderOrderEntity order = await _venderOrderRepository.GetAsync(message.OrderId);
            order.Status = message.AwardStatus;
            if (message.AwardStatus == OrderStatus.Awarding.Winning)
            {
                LotteryVenderEntities ldp = await _venderRepository.GetAsync(message.LdpVenderId);
                ldp.Balance = ldp.Balance - order.InvestAmount;
                ldp.TicketAmount = ldp.TicketAmount + order.InvestAmount;

                order.LdpVenderId = message.LdpVenderId;
                await _venderRepository.UpdateAsync(ldp);
                await _venderAccountDetailRepository.InsertAsync(new LotteryVenderAccountDetailEntity { Id = Guid.NewGuid().ToString("N"), VenderId = order.LdpVenderId, OrderId = order.LdpVenderId, LotteryId = order.LotteryId, Amount = order.InvestAmount, Status = order.Status, CretionTime = DateTime.Now });
                await _venderAccountDetailRepository.InsertAsync(new LotteryVenderAccountDetailEntity { Id = Guid.NewGuid().ToString("N"), VenderId = order.LdpVenderId, OrderId = order.LdpVenderId, LotteryId = order.LotteryId, Amount = order.InvestAmount, Status = order.Status, CretionTime = DateTime.Now });


                LotteryVenderEntities lvp = await _venderRepository.GetAsync(order.LvpVenderId);
                lvp.Balance = lvp.Balance + order.InvestAmount;
                lvp.TicketAmount = lvp.TicketAmount - order.InvestAmount;
                await _venderRepository.UpdateAsync(lvp);
                await _venderAccountDetailRepository.InsertAsync(new LotteryVenderAccountDetailEntity { Id = Guid.NewGuid().ToString("N"), VenderId = order.LvpVenderId, OrderId = order.LvpOrderId, LotteryId = order.LotteryId, Amount = order.InvestAmount, Status = order.Status, CretionTime = DateTime.Now });
                await _venderAccountDetailRepository.InsertAsync(new LotteryVenderAccountDetailEntity { Id = Guid.NewGuid().ToString("N"), VenderId = order.LvpVenderId, OrderId = order.LvpOrderId, LotteryId = order.LotteryId, Amount = order.InvestAmount, Status = order.Status, CretionTime = DateTime.Now });
            }
            ICache cacher = _cacheManager.GetCache("LotteryVender.Orders");
            await cacher.SetAsync(order.Id, Task.FromResult(order));
            await _venderOrderRepository.UpdateAsync(order);
            return true;
        }
    }
}
