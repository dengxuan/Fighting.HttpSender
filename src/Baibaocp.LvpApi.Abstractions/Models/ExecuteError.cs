namespace Baibaocp.LvpApi.Models
{
    public class ExecuteError
    {
        /// <summary>
        /// Error code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Error details.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="ExecuteError"/>.
        /// </summary>
        public ExecuteError()
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="ExecuteError"/>.
        /// </summary>
        /// <param name="message">Error message</param>
        public ExecuteError(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ExecuteError"/>.
        /// </summary>
        /// <param name="code">Error code</param>
        public ExecuteError(int code)
        {
            Code = code;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ExecuteError"/>.
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Error message</param>
        public ExecuteError(int code, string message) : this(message)
        {
            Code = code;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInformation"/>.
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="details">Error details</param>
        public ExecuteError(string message, string details) : this(message)
        {
            Details = details;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorInformation"/>.
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Error message</param>
        /// <param name="details">Error details</param>
        public ExecuteError(int code, string message, string details) : this(message, details)
        {
            Code = code;
        }
    }
}
