using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryVender.Sending.Abstractions
{
    public interface ISenderFactory<TEntity>
    {
        IReadOnlyList<ISender<TEntity>> GetSender(int LotteryId);
    }
}
