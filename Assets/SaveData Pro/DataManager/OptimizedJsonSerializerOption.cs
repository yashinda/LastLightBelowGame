namespace SaveDataPro.DataManager
{
    using UnityEngine;
    using Newtonsoft.Json;
    using System.Text;
    using System.IO;

    /// <summary>
    /// Optimized version of JsonSerializerOption
    /// Minimizes garbage collection and improves performance
    /// </summary>
    public class OptimizedJsonSerializerOption : ISerializationOption
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.None,  // No formatting to save space
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,  // Ignore null values
            DefaultValueHandling = DefaultValueHandling.Ignore  // Ignore default values
        };

        // Reuse JsonSerializer instance to avoid constant recreation
        private static readonly JsonSerializer serializer = JsonSerializer.Create(settings);

        // StringBuilder pool for reuse
        private static readonly StringBuilder stringBuilder = new StringBuilder(1024);

        public byte[] Serialize<T>(T obj)
        {
            try
            {
                // Clear StringBuilder for reuse
                stringBuilder.Clear();

                using (var stringWriter = new StringWriter(stringBuilder))
                using (var jsonWriter = new JsonTextWriter(stringWriter))
                {
                    serializer.Serialize(jsonWriter, obj);
                    string jsonString = stringBuilder.ToString();
                    return Encoding.UTF8.GetBytes(jsonString);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Optimized JSON Serialization failed: {e.Message}");
                throw;
            }
        }

        public T Deserialize<T>(byte[] data)
        {
            try
            {
                string jsonString = Encoding.UTF8.GetString(data);

                using (var stringReader = new StringReader(jsonString))
                using (var jsonReader = new JsonTextReader(stringReader))
                {
                    return serializer.Deserialize<T>(jsonReader);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Optimized JSON Deserialization failed: {e.Message}");
                throw;
            }
        }
    }
}
