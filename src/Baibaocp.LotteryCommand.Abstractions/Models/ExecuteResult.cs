using Baibaocp.LotteryCommand.Models;

namespace Baibaocp.LotteryCommand.Abstractions
{
    public class ExecuteResult
    {
        public bool Success { get; set; }

        public ExecuteError Error { get; set; }

        public ExecuteResult()
        {
            Success = true;
        }

        public ExecuteResult(bool success)
        {
            Success = success;
        }

        public ExecuteResult(ExecuteError error)
        {
            Error = error;
            Success = false;
        }
    }
}