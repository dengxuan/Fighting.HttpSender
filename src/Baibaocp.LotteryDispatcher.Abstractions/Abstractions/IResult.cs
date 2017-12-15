using Baibaocp.Core;

namespace Baibaocp.LotteryDispatcher.Abstractions
{
    public interface IResult
    {
        OrderStatus Code { get; }

        string Message { get; }
    }
}
