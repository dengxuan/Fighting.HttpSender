using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fighting.Worker.Storage.Abstractions
{
    public interface ICommandWrapper: IDisposable
    {
        string CommandText { get; set; }

        void AddParameterWithValue(string parameterName, object value);

        Task<int> ExecuteNonQueryAsync(CancellationToken ct);

        Task<object> ExecuteScalarAsync(CancellationToken ct);
    }
}
