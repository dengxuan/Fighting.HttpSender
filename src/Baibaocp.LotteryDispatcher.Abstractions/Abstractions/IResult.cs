using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryDispatcher.Abstractions
{
    public interface IResult
    {
        int Code { get; }

        string Message { get; }
    }
}
