namespace Baibaocp.LotteryDispatcher.Abstractions
{
    public class ExecuteResult<TResult> where TResult : IResult
    {

        public bool Success { get; set; }

        public TResult Result { get; set; }

        public ExecuteResult()
        {
            Success = true;
        }

        public ExecuteResult(bool success)
        {
            Success = success;
        }

        public ExecuteResult(TResult result)
        {
            Result = result;
            Success = true;
        }
    }
}