namespace SaveDataPro.DataManager
{
    using UnityEngine;
    using System.IO;

    /// <summary>
    /// Global configuration for SaveData Pro
    /// Allows adjustment of security and performance parameters
    /// </summary>
    [System.Serializable]
    public class SaveDataProConfig
    {
        [Header("Encryption Settings")]
        [Tooltip("AES key size (32 = 256-bit, 24 = 192-bit, 16 = 128-bit)")]
        public int aesKeySize = 32; // 256-bit default
        
        [Tooltip("AES IV size (always 16 bytes for AES)")]
        public int aesIvSize = 16; // 128-bit
        
        [Tooltip("Default encryption key (will be hashed to AES key)")]
        public string defaultEncryptionKey = "SaveData_Pro_Unity_2025_SecretKey";

        [Header("Data Integrity")]
        [Tooltip("Secret salt for hash computation")]
        public string secretSalt = "SaveData_Pro_Salt_2025_Unity";
        
        [Tooltip("Enable/disable data integrity checking")]
        public bool enableIntegrityCheck = true;

        [Header("Serialization")]
        [Tooltip("Default serialization type")]
        public SerializationType defaultSerializationType = SerializationType.Json;
        
        [Tooltip("Enable optimization for JSON serializer")]
        public bool useOptimizedJson = false;

        [Header("File System")]
        [Tooltip("Custom save directory (leave empty to use default)")]
        public string customSaveDirectory = "";
        
        [Tooltip("File name prefix")]
        public string filePrefix = "";
        
        [Tooltip("File extension")]
        public string fileExtension = ".sav";

        [Header("Performance")]
        [Tooltip("Maximum cache size (MB)")]
        public int maxCacheSizeMB = 50;
        
        [Tooltip("Enable auto-save")]
        public bool enableAutoSave = false;
        
        [Tooltip("Auto-save interval (seconds)")]
        public float autoSaveInterval = 300f; // 5 minutes

        [Header("Debug")]
        [Tooltip("Enable verbose logging")]
        public bool enableVerboseLogging = false;
        
        [Tooltip("Save automatic backups")]
        public bool enableAutoBackup = true;
        
        [Tooltip("Maximum backup count")]
        public int maxBackupCount = 3;

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static SaveDataProConfig _instance;
        public static SaveDataProConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    LoadConfig();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Configuration file path
        /// </summary>
        private static string ConfigFilePath => Path.Combine(Application.persistentDataPath, "SaveDataProConfig.json");

        /// <summary>
        /// Load configuration from file or create default
        /// </summary>
        public static void LoadConfig()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    string json = File.ReadAllText(ConfigFilePath);
                    _instance = JsonUtility.FromJson<SaveDataProConfig>(json);
                    
                    if (Instance.enableVerboseLogging)
                    {
                        Debug.Log("SaveData Pro config loaded successfully");
                    }
                }
                else
                {
                    _instance = new SaveDataProConfig();
                    SaveConfig();
                    Debug.Log("Created default SaveData Pro config");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load SaveData Pro config: {e.Message}");
                _instance = new SaveDataProConfig();
            }
        }

    /// <summary>
    /// Save configuration to file
    /// </summary>
        public static void SaveConfig()
        {
            try
            {
                string json = JsonUtility.ToJson(_instance, true);
                File.WriteAllText(ConfigFilePath, json);
                
                if (Instance.enableVerboseLogging)
                {
                    Debug.Log("SaveData Pro config saved successfully");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to save SaveData Pro config: {e.Message}");
            }
        }

    /// <summary>
    /// Reset to default configuration
    /// </summary>
        public static void ResetToDefault()
        {
            _instance = new SaveDataProConfig();
            SaveConfig();
            Debug.Log("SaveData Pro config reset to default");
        }

        /// <summary>
        /// Create platform-optimized configuration
        /// </summary>
        public static SaveDataProConfig CreatePlatformOptimized()
        {
            var config = new SaveDataProConfig();
            
            // Apply platform-specific optimizations
    #if UNITY_WEBGL
            config.useOptimizedJson = true;
            config.enableVerboseLogging = false;
            config.enableAutoBackup = false;
            config.maxCacheSizeMB = 10;
            config.enableAutoSave = false;
            config.fileExtension = ".dat";
    #elif UNITY_ANDROID || UNITY_IOS
            config.maxBackupCount = 2;
            config.enableVerboseLogging = false;
            config.maxCacheSizeMB = 25;
            config.enableAutoSave = true;
            config.autoSaveInterval = 180f; // 3 minutes
    #elif UNITY_STANDALONE
            config.maxBackupCount = 5;
            config.enableVerboseLogging = true;
            config.maxCacheSizeMB = 100;
            config.enableAutoSave = true;
            config.autoSaveInterval = 300f; // 5 minutes
    #endif
            
            return config;
        }

    /// <summary>
    /// Validate configuration
    /// </summary>
        public bool ValidateConfig()
        {
            bool isValid = true;

            // Check AES key size
            if (aesKeySize != 16 && aesKeySize != 24 && aesKeySize != 32)
            {
                Debug.LogError("Invalid AES key size. Must be 16, 24, or 32 bytes");
                isValid = false;
            }

            // Check AES IV size
            if (aesIvSize != 16)
            {
                Debug.LogError("Invalid AES IV size. Must be 16 bytes");
                isValid = false;
            }

            // Check encryption key
            if (string.IsNullOrEmpty(defaultEncryptionKey))
            {
                Debug.LogError("Default encryption key cannot be empty");
                isValid = false;
            }

            // Check secret salt
            if (string.IsNullOrEmpty(secretSalt))
            {
                Debug.LogError("Secret salt cannot be empty");
                isValid = false;
            }

            // Check auto-save interval
            if (enableAutoSave && autoSaveInterval <= 0)
            {
                Debug.LogError("Auto-save interval must be greater than 0");
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Get save directory path
        /// </summary>
        public string GetSaveDirectory()
        {
            if (!string.IsNullOrEmpty(customSaveDirectory))
            {
                return customSaveDirectory;
            }
            
            // Platform-specific directory optimization
            string baseDir = Application.persistentDataPath;
            
    #if UNITY_WEBGL
            return Path.Combine(baseDir, "SD"); // Shorter path for web
    #elif UNITY_ANDROID || UNITY_IOS
            return Path.Combine(baseDir, "SaveData_Pro"); // Standard mobile path
    #elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
            return Path.Combine(baseDir, "SaveData_Pro", "GameData"); // Desktop can use longer paths
    #else
            return Path.Combine(baseDir, "SaveData_Pro"); // Default fallback
    #endif
        }
    }

    /// <summary>
    /// Enum of serialization types
    /// </summary>
    public enum SerializationType
    {
        Json,
        OptimizedJson,
        SafeBinary
    }

}