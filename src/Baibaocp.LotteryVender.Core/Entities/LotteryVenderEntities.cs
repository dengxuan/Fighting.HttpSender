using Fighting.Storaging.Entities.Abstractions;
using System;

namespace Baibaocp.LotteryVender.Core.Entities
{
    public class LotteryVenderEntities : Entity<string>
    {
        public string Name { get; set; }

        public string SecretKey { get; set; }

        public decimal Balance { get; set; }

        public decimal TicketAmount { get; set; }

        public decimal RewardAmount { get; set; }

        public int ChannelTypeId { get; set; }

        public string Address { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
