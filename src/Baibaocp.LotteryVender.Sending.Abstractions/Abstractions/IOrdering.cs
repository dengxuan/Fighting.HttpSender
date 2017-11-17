using System.Threading.Tasks;

namespace Baibaocp.LotteryVender.Sending.Abstractions
{
    public interface IOrdering<in TOrder> : ISender<TOrder> where TOrder : IOrder
    {
    }
}
