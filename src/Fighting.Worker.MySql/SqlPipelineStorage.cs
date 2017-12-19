using Fighting.Worker.Abstractions;
using Fighting.Worker.Storage.Abstractions;
using Hangfire.Logging;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fighting.Worker.MySql
{

    /// <summary>
    /// SQL Server implementation of Hangfire pipeline storage
    /// </summary>
    public class SqlPipelineStorage : IWorkerStorage
    {
        private static readonly ILog Log = LogProvider.GetLogger(typeof(SqlPipelineStorage));
        private readonly WorkerStorageOptions _options;

        public SqlPipelineStorage(WorkerStorageOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Creates a job context record in SQL Server, job context ID is used as the primary key
        /// </summary>
        public async Task<bool> CreateJobContextAsync(IWorkerContext workerContext, CancellationToken ct)
        {
            Log.DebugFormat("Creating job context '{0}' record in SQL Server", workerContext.Id);
            var serialized = Serialize(workerContext);
            var bytes = GetBytes(serialized);
            if (_options.UseCompression)
                bytes = _options.Compression.CompressBytes(bytes);
            using (var conn = GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"insert into {_options.Table} ({_options.KeyColumn},{_options.ValueColumn}) values (@key,@value)";
                cmd.AddParameterWithValue("key", workerContext.Id);
                cmd.AddParameterWithValue("value", bytes);
                await conn.OpenAsync(ct);
                try
                {
                    Log.Debug("Executing SQL Server INSERT command");
                    var rowsAffected = await cmd.ExecuteNonQueryAsync(ct);
                    return rowsAffected > 0;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public async Task<bool> DeleteJobContextAsync(string id, CancellationToken ct)
        {
            Log.DebugFormat("Getting job context '{0}' from SQL Server", id);
            using (var conn = GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"delete from {_options.Table} where {_options.KeyColumn}=@key";
                cmd.AddParameterWithValue("key", id);
                await conn.OpenAsync(ct);
                try
                {
                    Log.Debug("Executing SQL Server DELETE command");
                    var rowsAffected = await cmd.ExecuteNonQueryAsync(ct);
                    return rowsAffected > 0;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Gets a job context record by ID from SQL Server
        /// </summary>
        public async Task<IWorkerContext> GetWorkerContextAsync(string id, CancellationToken ct)
        {
            Log.DebugFormat("Getting job context '{0}' from SQL Server", id);
            byte[] bytes;
            using (var conn = GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"select top 1 {_options.ValueColumn} from {_options.Table} where {_options.KeyColumn}=@key";
                cmd.AddParameterWithValue("key", id);
                await conn.OpenAsync(ct);
                try
                {
                    Log.Debug("Executing SQL Server SELECT command");
                    var res = await cmd.ExecuteScalarAsync(ct);
                    bytes = (byte[])res;
                }
                finally
                {
                    conn.Close();
                }
            }
            if (bytes == null)
                return null;
            if (_options.UseCompression)
                bytes = _options.Compression.DecompressBytes(bytes);
            var serialized = GetString(bytes);
            var jobContext = Deserialize(serialized);
            return jobContext;
        }

        /// <summary>
        /// Updates a job context record in SQL Server, job context ID is used as the primary key
        /// </summary>
        public async Task<bool> UpdateJobContextAsync(IWorkerContext workerContext, CancellationToken ct)
        {
            Log.DebugFormat("Updating job context '{0}' in SQL Server", workerContext.Id);
            var serialized = Serialize(workerContext);
            var bytes = GetBytes(serialized);
            if (_options.UseCompression)
                bytes = _options.Compression.CompressBytes(bytes);
            using (var conn = GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"update {_options.Table} set {_options.ValueColumn}=@value where {_options.KeyColumn}=@key";
                cmd.AddParameterWithValue("key", workerContext.Id);
                cmd.AddParameterWithValue("value", bytes);
                await conn.OpenAsync(ct);
                try
                {
                    Log.Debug("Executing SQL Server UPDATE command");
                    var rowsAffected = await cmd.ExecuteNonQueryAsync(ct);
                    return rowsAffected > 0;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        protected virtual IConnectionWrapper GetConnection()
        {
            Log.Debug("Creating SQL Server connection");
            var conn = _options.ConnectionFactory.Create();
            return conn;
        }

        protected virtual string Serialize(IWorkerContext workerContext)
        {
            return _options.Serializer.Serialize(workerContext);
        }

        protected virtual IWorkerContext Deserialize(string json)
        {
            return _options.Serializer.Deserialize(json);
        }

        protected virtual byte[] GetBytes(string serialized)
        {
            var binary = Encoding.UTF8.GetBytes(serialized);
            return binary;
        }

        protected virtual string GetString(byte[] bytes)
        {
            var serialized = Encoding.UTF8.GetString(bytes);
            return serialized;
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
