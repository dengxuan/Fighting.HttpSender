namespace Fighting.Worker.Abstractions
{
    /// <summary>
    /// A serializer for pipeline jobs
    /// </summary>
    public interface IWorkerSerializer
    {
        /// <summary>
        /// Deserialize s JSON string to a job context
        /// </summary>
        IWorkerContext Deserialize(string serialized);

        /// <summary>
        /// Serialize a job context to a JSON string
        /// </summary>
        string Serialize(IWorkerContext jobContext);
    }
}