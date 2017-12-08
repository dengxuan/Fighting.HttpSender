using Fighting.Storaging.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryVender.Core.Entities
{
    public class LotteryVenderAccountDetailEntity : Entity<string>
    {
        public string VenderId { get; set; }

        public string OrderId { get; set; }

        public int LotteryId { get; set; }

        public int Amount { get; set; }

        public int Status { get; set; }

        public DateTime CretionTime { get; set; }
    }
}
