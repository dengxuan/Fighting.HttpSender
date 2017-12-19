using System;

namespace Fighting.Worker.Storage.Abstractions
{
    /// <summary>
    /// A factory for creating and releasing SQL connection
    /// </summary>
    public interface IConnectionFactory : IDisposable
    {
        IConnectionWrapper Create();

        void Release(IConnectionWrapper sqlConnection);
    }
}