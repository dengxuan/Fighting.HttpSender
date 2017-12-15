using Baibaocp.Core;

namespace Baibaocp.LotteryDispatcher.Abstractions
{
    public abstract class Result : IResult
    {
        /// <summary>
        /// Result code.
        /// </summary>
        public OrderStatus Code { get; set; }

        /// <summary>
        /// Result message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="Result"/>.
        /// </summary>
        /// <param name="code">Error code</param>
        public Result(OrderStatus code)
        {
            Code = code;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Result"/>.
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Error message</param>
        public Result(OrderStatus code, string message) : this(code)
        {
            Message = message;
        }
    }
}
