using UnityEngine;

namespace SaveDataPro.DataManager
{
    /// <summary>
    /// Base interface for data providers
    /// Follows Dependency Injection principle to ensure modularity and testability
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// Save data with specified key
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="key">Unique key</param>
        /// <param name="data">Data to save</param>
        void Save<T>(string key, T data);

        /// <summary>
        /// Load data by key
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="key">Unique key</param>
        /// <param name="defaultValue">Default value if not found</param>
        /// <returns>Loaded data or default value</returns>
        T Load<T>(string key, T defaultValue = default);

        /// <summary>
        /// Check if data exists
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if exists</returns>
        bool Exists(string key);

        /// <summary>
        /// Delete data by key
        /// </summary>
        /// <param name="key">Key to delete</param>
        void Delete(string key);
    }
}
