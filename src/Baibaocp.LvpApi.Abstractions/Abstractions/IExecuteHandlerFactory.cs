using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.LvpApi.Abstractions
{
    public interface IExecuteHandlerFactory
    {
        Task<IExecuteHandler<TExecuter>> GetHandlerAsync<TExecuter>(string lvpVenderId) where TExecuter : IExecuter;
    }
}
