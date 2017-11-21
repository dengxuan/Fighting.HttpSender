using Baibaocp.LotterySales.Core.Entities;
using Baibaocp.LotterySales.WebApi.Models;
using Baibaocp.LotteryVender.Messaging;
using Baibaocp.LotteryVender.WebApi.Entity;
using Fighting.Extensions.Caching.Abstractions;
using Fighting.Extensions.Messaging.Abstractions;
using Fighting.Storaging.Repositories.Abstractions;
using Fighting.WebApi.Abstractions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryVender.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {

        private readonly ICacheManager _cacheManager;

        private readonly IMessagePublisher _messagePublisher;

        private readonly IRepository<LotterySalesOrderEntity, string> _repository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="cacheManager"></param>
        /// <param name="messagePublisher"></param>
        public OrdersController(IRepository<LotterySalesOrderEntity, string> repository, ICacheManager cacheManager, IMessagePublisher messagePublisher)
        {
            _repository = repository;
            _cacheManager = cacheManager;
            _messagePublisher = messagePublisher;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [WrapResult]
        [AllowAnonymous]
        public async Task<OrderOutput> GetOrderAsync(string id)
        {
            ICache cache = _cacheManager.GetCache("Baibaocp.LotterySales.OrdersStatus");
            LotterySalesOrderEntity entity = await cache.GetAsync(id, cackeKey =>
            {
                return _repository.GetAll().Where(predicate => predicate.ChannelOrderId == cackeKey).FirstOrDefault();
            });
            if (entity == null)
            {
                return null;
            }
            return new OrderOutput
            {
                OrderId = id,
                AftertaxBonusAmount = entity.AftertaxBonusAmount,
                BonusAmount = entity.BonusAmount,
                InvestAmount = entity.InvestAmount,
                InvestCode = entity.InvestCode,
                InvestCount = entity.InvestCount,
                CreationTime = entity.CreationTime,
                InvestTimes = entity.InvestTimes,
                InvestType = entity.InvestType ? 1 : 0,
                IssueNumber = entity.IssueNumber,
                LotteryId = entity.LotteryId,
                LotteryPlayId = entity.LotteryPlayId,
                Status = entity.Status,
                UserId = entity.LvpUserId
            };
        }

        /// <summary>
        /// 投注
        /// </summary>
        /// <param name="order">订单</param>
        [HttpPost]
        [WrapResult]
        public async Task PostAsync([FromBody] OrderInput order)
        {
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                OrderingMessage message = new OrderingMessage
                {
                    Id = order.OrderId,
                    LvpUserId = order.UserId,
                    LvpVenderId = "100010",
                    LotteryId = order.LotteryId,
                    LotteryPlayId = order.LotteryPlayId,
                    IssueNumber = order.IssueNumber,
                    InvestType = order.InvestType,
                    InvestCode = order.InvestCode,
                    InvestCount = order.InvestCount,
                    InvestTimes = order.InvestTimes,
                    InvestAmount = order.InvestAmount,
                    CreationTime = DateTime.Now
                };
                await _messagePublisher.Publish(message, CancellationToken.None);
            }
        }
    }
}
