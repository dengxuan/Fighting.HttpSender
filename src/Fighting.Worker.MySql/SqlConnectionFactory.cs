using Fighting.Worker.Storage.Abstractions;
using Hangfire.Logging;
using Pomelo.Data.MySql;
using System;

namespace Fighting.Worker.MySql
{
    /// <summary>
    /// A factory for creating and releasing SQL connection
    /// </summary>
    public class SqlConnectionFactory : IConnectionFactory
    {
        private static readonly ILog Log = LogProvider.GetLogger(typeof(SqlConnectionFactory));

        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            _connectionString = connectionString;
        }

        public IConnectionWrapper Create()
        {
            Log.Debug("Creating new SQL Server connection");
            var connection = new MySqlConnection(_connectionString);
            return new SqlConnectionWrapper(connection);
        }

        public void Release(IConnectionWrapper sqlConnection)
        {
            Log.Debug("Releasing SQL Server connection");
            sqlConnection?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
