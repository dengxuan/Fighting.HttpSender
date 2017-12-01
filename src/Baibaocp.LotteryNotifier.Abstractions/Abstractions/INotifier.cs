namespace Baibaocp.LotteryNotifier.Abstractions
{
    public interface INotifier
    {
        string OrderId { get; set; }

        string LvpVenderId { get; set; }
    }
}
