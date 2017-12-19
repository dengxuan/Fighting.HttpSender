using System;

namespace Fighting.Worker.Abstractions
{
    /// <summary>
    /// A scope object for use with DI and inversion of control container
    /// </summary>
    public interface IWorkerTaskFactoryScope : IDisposable
    {
    }
}
