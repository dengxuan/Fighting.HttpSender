using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Abstractions
{
    public interface INoticeDispatcher
    {
        Task<bool> DispatchAsync<TNotifier>(TNotifier notifier) where TNotifier : INotifier;
    }
}
