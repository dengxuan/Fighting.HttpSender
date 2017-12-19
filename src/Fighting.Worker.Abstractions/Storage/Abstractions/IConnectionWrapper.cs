using Fighting.Worker.Storage.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fighting.Worker.Storage.Abstractions
{
    public interface IConnectionWrapper : IDisposable
    {
        void Close();

        Task OpenAsync(CancellationToken ct);

        ICommandWrapper CreateCommand();
    }
}