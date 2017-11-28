using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryDispatcher.Abstractions
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HandlerAttribute : Attribute
    {
        public string LdpVenderId { get; set; }

        public string Key { get; set; }

        public string Url { get; set; }
    }
}
