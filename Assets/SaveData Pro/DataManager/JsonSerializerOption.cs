namespace SaveDataPro.DataManager
{
    using UnityEngine;
    using Newtonsoft.Json;
    using System.Text;

    /// <summary>
    /// JSON serialization implementation using Newtonsoft.Json
    /// Supports Dictionary and complex data types
    /// </summary>
    public class JsonSerializerOption : ISerializationOption
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto, // Support polymorphism
            Formatting = Formatting.Indented, // Easy to read when debugging
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore, // Avoid reference loops
            NullValueHandling = NullValueHandling.Include // Include null values
        };

        public byte[] Serialize<T>(T obj)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(obj, settings);
                return Encoding.UTF8.GetBytes(jsonString);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"JSON Serialization failed: {e.Message}");
                throw;
            }
        }

        public T Deserialize<T>(byte[] data)
        {
            try
            {
                string jsonString = Encoding.UTF8.GetString(data);
                return JsonConvert.DeserializeObject<T>(jsonString, settings);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"JSON Deserialization failed: {e.Message}");
                throw;
            }
        }
    }
}
