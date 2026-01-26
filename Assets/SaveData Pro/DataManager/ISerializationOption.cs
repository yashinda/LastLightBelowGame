namespace SaveDataPro.DataManager
{
    /// <summary>
    /// Interface for data serialization options
    /// Supports Strategy Pattern to allow switching between serialization methods
    /// </summary>
    public interface ISerializationOption
    {
        /// <summary>
        /// Serialize object to byte array
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="obj">Object to serialize</param>
        /// <returns>Serialized byte array</returns>
        byte[] Serialize<T>(T obj);

        /// <summary>
        /// Deserialize byte array to object
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="data">Byte array to deserialize</param>
        /// <returns>Deserialized object</returns>
        T Deserialize<T>(byte[] data);
    }
}
