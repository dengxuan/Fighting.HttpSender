﻿using Baibaocp.LotteryNotifier.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryNotifier
{
    public class NoticeConfiguration
    {
        public string LvpVenderId { get; set; }

        public string Url { get; set; }

        public string SecurityKey { get; set; }
    }
}
