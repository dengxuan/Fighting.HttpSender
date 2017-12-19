using Fighting.Worker.Storage.Abstractions;
using Pomelo.Data.MySql;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fighting.Worker.MySql
{

    public class SqlCommandWrapper : ICommandWrapper
    {
        private readonly MySqlCommand _command;

        public SqlCommandWrapper(MySqlCommand command)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
        }

        public virtual string CommandText
        {
            get { return _command.CommandText; }
            set { _command.CommandText = value; }
        }

        public void AddParameterWithValue(string parameterName, object value)
        {
            _command.Parameters.AddWithValue(parameterName, value);
        }

        public async Task<int> ExecuteNonQueryAsync(CancellationToken ct)
        {
            return await _command.ExecuteNonQueryAsync(ct);
        }

        public async Task<object> ExecuteScalarAsync(CancellationToken ct)
        {
            return await _command.ExecuteScalarAsync(ct);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _command?.Dispose();
        }
    }
}
