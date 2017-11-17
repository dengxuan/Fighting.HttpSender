using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.LotteryVender.Sending.Abstractions
{
    public interface ISender<in TEntity>
    {
        bool Send(TEntity entity);

        Task<bool> SendAsync(TEntity entity);
    }
}
