using Fighting.Worker.Storage.Abstractions;
using Pomelo.Data.MySql;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fighting.Worker.MySql
{
    public class SqlConnectionWrapper : IConnectionWrapper
    {
        private readonly MySqlConnection _connection;

        public SqlConnectionWrapper(MySqlConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            _connection = connection;
        }

        public async Task OpenAsync(CancellationToken ct)
        {
            await _connection.OpenAsync(ct);
        }

        ICommandWrapper IConnectionWrapper.CreateCommand()
        {
            var command = _connection.CreateCommand();
            return new SqlCommandWrapper(command);
        }

        public void Close()
        {
            _connection.Close();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _connection?.Dispose();
        }
    }
}
