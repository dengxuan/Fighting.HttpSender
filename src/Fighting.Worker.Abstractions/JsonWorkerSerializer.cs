using Fighting.Worker.Abstractions;
using Newtonsoft.Json;
using System;

namespace Fighting.Worker
{
    /// <summary>
    /// A JSON.Net implementation of the pipeline serializer
    /// </summary>
    public class JsonWorkerSerializer : IWorkerSerializer
    {
        private readonly Type _jobContextType;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly JsonConverter[] _converters;

        public JsonWorkerSerializer(Type jobContextType,
            JsonSerializerSettings jsonSerializerSettings)
        {
            if (jobContextType == null)
                throw new ArgumentNullException(nameof(jobContextType));
            if (jsonSerializerSettings == null)
                throw new ArgumentNullException(nameof(jsonSerializerSettings));
            _jobContextType = jobContextType;
            _jsonSerializerSettings = jsonSerializerSettings;
        }

        public JsonWorkerSerializer()
            : this(typeof(WorkerContext), new JsonSerializerSettings
            {
                Converters = new[] { new WorkerTaskContextJsonConverter()
            }})
        {
        }

        /// <summary>
        /// Deserializes a JSON string to a pipeline job context
        /// </summary>
        public IWorkerContext Deserialize(string json)
        {
            var deserialized = JsonConvert.DeserializeObject(json, _jobContextType,
                _jsonSerializerSettings);
            return (IWorkerContext)deserialized;
        }

        /// <summary>
        /// Serializes a pipeline job context to JSON
        /// </summary>
        public string Serialize(IWorkerContext jobContext)
        {
            var serialized = JsonConvert.SerializeObject(jobContext, _jsonSerializerSettings);
            return serialized;
        }
    }
}
