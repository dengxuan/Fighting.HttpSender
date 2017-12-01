using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Abstractions
{
    public interface INoticeHandler<TNotifier> where TNotifier : INotifier
    {
        Task<bool> HandleAsync(TNotifier notifier);
    }
}
