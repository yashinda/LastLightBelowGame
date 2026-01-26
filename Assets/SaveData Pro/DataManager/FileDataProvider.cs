using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace SaveDataPro.DataManager
{
    /// <summary>
    /// File-system based data provider implementation
    /// Supports encryption and data integrity verification
    /// </summary>
    public class FileDataProvider : IDataProvider
    {
        private readonly string saveDirectory;
        private readonly ISerializationOption serializer;
        private readonly IEncryptionOption encryption;
        private readonly bool enableIntegrityCheck;

    // Cache to store hashes of saved files
        private readonly Dictionary<string, string> hashCache = new Dictionary<string, string>();

        public FileDataProvider(
            ISerializationOption serializer = null,
            IEncryptionOption encryption = null,
            bool enableIntegrityCheck = true)
        {
            var config = SaveDataProConfig.Instance;

            // Use directory from configuration or default
            this.saveDirectory = config.GetSaveDirectory();
            this.serializer = serializer ?? new JsonSerializerOption();
            this.encryption = encryption;
            this.enableIntegrityCheck = enableIntegrityCheck;

            // Ensure the save directory exists
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
                Debug.Log($"Created save directory: {saveDirectory}");
            }
        }

        public void Save<T>(string key, T data)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Save key cannot be null or empty");
                return;
            }

            string filePath = GetFilePath(key);
            string hashFilePath = GetHashFilePath(key);

            try
            {
                // 1. Serialize data
                byte[] serializedData = serializer.Serialize(data);

                // 2. Compute hash for integrity verification (before encryption)
                string dataHash = null;
                if (enableIntegrityCheck)
                {
                    dataHash = DataIntegrityUtility.ComputeHash(serializedData);
                    hashCache[key] = dataHash;
                }

                // 3. Encrypt data (if enabled)
                if (encryption != null)
                {
                    serializedData = encryption.Encrypt(serializedData);
                }

                // 4. Write data to file
                File.WriteAllBytes(filePath, serializedData);

                // 5. Write hash to a separate file (if enabled)
                if (enableIntegrityCheck && dataHash != null)
                {
                    File.WriteAllText(hashFilePath, dataHash);
                }

                Debug.Log($"Successfully saved data for key: {key}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to save data for key '{key}'. Error: {e.Message}");
                throw;
            }
        }

        public T Load<T>(string key, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Load key cannot be null or empty");
                return defaultValue;
            }

            string filePath = GetFilePath(key);
            string hashFilePath = GetHashFilePath(key);

            if (!File.Exists(filePath))
            {
                Debug.Log($"No save file found for key: {key}");
                return defaultValue;
            }

            try
            {
                // 1. Read raw data from file
                byte[] fileData = File.ReadAllBytes(filePath);

                // 2. Decrypt data (if enabled)
                if (encryption != null)
                {
                    fileData = encryption.Decrypt(fileData);
                }

                // 3. Verify data integrity (if enabled)
                if (enableIntegrityCheck && File.Exists(hashFilePath))
                {
                    string expectedHash = File.ReadAllText(hashFilePath);
                    if (!DataIntegrityUtility.VerifyIntegrity(fileData, expectedHash))
                    {
                        Debug.LogWarning($"Data integrity check failed for key '{key}'. File may have been tampered with.");
                        return defaultValue;
                    }
                }

                // 4. Deserialize data
                T result = serializer.Deserialize<T>(fileData);
                Debug.Log($"Successfully loaded data for key: {key}");
                return result;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load data for key '{key}'. Error: {e.Message}. Returning default value.");
                return defaultValue;
            }
        }

        public bool Exists(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            return File.Exists(GetFilePath(key));
        }

        public void Delete(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Delete key cannot be null or empty");
                return;
            }

            string filePath = GetFilePath(key);
            string hashFilePath = GetHashFilePath(key);

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Debug.Log($"Deleted save file for key: {key}");
                }

                if (File.Exists(hashFilePath))
                {
                    File.Delete(hashFilePath);
                }

                // Remove from cache
                if (hashCache.ContainsKey(key))
                {
                    hashCache.Remove(key);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to delete data for key '{key}'. Error: {e.Message}");
            }
        }

    /// <summary>
    /// Get a list of all saved keys
    /// </summary>
        public string[] GetAllKeys()
        {
            try
            {
                var keys = new List<string>();
                var files = Directory.GetFiles(saveDirectory, "*.sav");

                foreach (var file in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    keys.Add(fileName);
                }

                return keys.ToArray();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to get all keys. Error: {e.Message}");
                return new string[0];
            }
        }

        private string GetFilePath(string key)
        {
            var config = SaveDataProConfig.Instance;
            // Use hash of the key to avoid invalid filename characters
            string safeKey = key.GetHashCode().ToString();
            string fileName = $"{config.filePrefix}{safeKey}{config.fileExtension}";
            return Path.Combine(saveDirectory, fileName);
        }

        private string GetHashFilePath(string key)
        {
            string safeKey = key.GetHashCode().ToString();
            return Path.Combine(saveDirectory, $"{safeKey}.hash");
        }
    }
}