using Baibaocp.LvpApi.Models;

namespace Baibaocp.LvpApi.Abstractions
{
    public class ExecuteResult
    {
        public bool Success { get; set; }

        public string VenderId { get; set; }

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