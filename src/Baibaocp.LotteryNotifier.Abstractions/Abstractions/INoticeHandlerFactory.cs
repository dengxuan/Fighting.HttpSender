namespace Baibaocp.LotteryNotifier.Abstractions
{
    public interface INoticeHandlerFactory
    {
        INoticeHandler<TNotifier> GetHandler<TNotifier>(NoticeConfiguration configure) where TNotifier : INotifier;
    }
}
