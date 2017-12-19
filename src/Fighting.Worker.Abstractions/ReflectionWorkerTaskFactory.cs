using Fighting.Worker.Abstractions;
using Hangfire.Logging;
using System;
using System.Reflection;

namespace Fighting.Worker
{

    public class ReflectionWorkerTaskFactory : IWorkerTaskFactory
    {
        private static readonly ILog Log = LogProvider.GetLogger(typeof(ReflectionWorkerTaskFactory));

        private readonly Assembly _assembly;

        public ReflectionWorkerTaskFactory(Assembly assembly)
        {
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        /// <summary>
        /// Creates a new task via reflection
        /// </summary>
        /// <param name="taskName">A full type name</param>
        public virtual IWorkerTask Create(string taskName)
        {
            Log.DebugFormat("Creating instance of '{0}' using reflection", taskName);
            var typ = _assembly.GetType(taskName);
            if (typ == null)
                throw new NullReferenceException($"Task '{taskName}' could not be resolved via reflection using assembly '{_assembly.FullName}', check your assembly and task name");
            var task = (IWorkerTask)Activator.CreateInstance(typ);
            return task;
        }

        /// <summary>
        /// Releases a task instance
        /// </summary>
        public virtual void Release(IWorkerTask taskInstance)
        {
            if (taskInstance != null)
            {
                Log.DebugFormat("Releasing instance of '{0}'", taskInstance.GetType().FullName);
                taskInstance.Dispose();
            }
        }

        /// <summary>
        /// This task factory does not use scoping
        /// </summary>
        public IWorkerTaskFactoryScope GetScope()
        {
            return null;
        }
    }
}
