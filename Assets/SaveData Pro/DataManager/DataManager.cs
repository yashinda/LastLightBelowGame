using UnityEngine;
using SaveDataPro.DataManager;

namespace SaveDataPro.DataManager
{
    /// <summary>
    /// Main data manager class - single access point for all data operations
    /// Uses the Dependency Injection pattern to ensure modularity and testability
    /// </summary>
    public static class DataManager
    {
        private static IDataProvider dataProvider;
        private static bool isInitialized = false;

    /// <summary>
    /// Initialize DataManager with default configuration
    /// </summary>
        static DataManager()
        {
            InitializeDefault();
        }

        /// <summary>
        /// Initialize with default configuration from SaveDataProConfig
        /// </summary>
        public static void InitializeDefault()
        {
            var config = SaveDataProConfig.Instance;

            // Choose serializer based on configuration
            ISerializationOption serializer = config.defaultSerializationType switch
            {
                SerializationType.Json => config.useOptimizedJson ?
                    new OptimizedJsonSerializerOption() : new JsonSerializerOption(),
                SerializationType.OptimizedJson => new OptimizedJsonSerializerOption(),
                SerializationType.SafeBinary => new SafeBinarySerializerOption(),
                _ => new JsonSerializerOption()
            };

            // Create encryption with key from configuration
            var encryption = new AesEncryptionOption(config.defaultEncryptionKey);

            // Create provider with settings from configuration
            var provider = new FileDataProvider(serializer, encryption, config.enableIntegrityCheck);

            Initialize(provider);
        }

    /// <summary>
    /// Initialize with platform-optimized settings
    /// </summary>
        public static void InitializePlatformOptimized()
        {
            var config = SaveDataProConfig.CreatePlatformOptimized();

            // Choose serializer based on platform
            ISerializationOption serializer;

    #if UNITY_WEBGL
            serializer = new OptimizedJsonSerializerOption(); // Performance critical for web
    #elif UNITY_ANDROID || UNITY_IOS
            serializer = config.useOptimizedJson ? new OptimizedJsonSerializerOption() : new JsonSerializerOption();
    #else
            serializer = config.defaultSerializationType switch
            {
                SerializationType.Json => new JsonSerializerOption(),
                SerializationType.OptimizedJson => new OptimizedJsonSerializerOption(),
                SerializationType.SafeBinary => new SafeBinarySerializerOption(),
                _ => new JsonSerializerOption()
            };
    #endif

            // Platform-specific encryption
            IEncryptionOption encryption = null;

    #if !UNITY_WEBGL  // Disable encryption on WebGL for performance
            encryption = new AesEncryptionOption($"SaveData_Pro_{Application.platform}_{System.DateTime.Now.Year}");
    #endif

            // Create provider with platform-optimized settings
            var provider = new FileDataProvider(serializer, encryption, config.enableIntegrityCheck);

            Initialize(provider);

            Debug.Log($"SaveData_Pro initialized with platform optimization for: {Application.platform}");
            Debug.Log($"Serialization: {(serializer is OptimizedJsonSerializerOption ? "Optimized JSON" : serializer.GetType().Name)}");
            Debug.Log($"Encryption: {(encryption != null ? "Enabled" : "Disabled")}");
        }

    /// <summary>
    /// Initialize with custom data provider
    /// </summary>
    /// <param name="provider">Data provider</param>
        public static void Initialize(IDataProvider provider)
        {
            dataProvider = provider ?? throw new System.ArgumentNullException(nameof(provider));
            isInitialized = true;

            Debug.Log($"DataManager initialized with provider: {provider.GetType().Name}");
        }

    /// <summary>
    /// Save data with the specified key
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    /// <param name="key">Unique key</param>
    /// <param name="data">Data to save</param>
        public static void Save<T>(string key, T data)
        {
            EnsureInitialized();

            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Save key cannot be null or empty");
                return;
            }

            try
            {
                dataProvider.Save(key, data);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"DataManager.Save failed for key '{key}': {e.Message}");
            }
        }

    /// <summary>
    /// Load data by key
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    /// <param name="key">Unique key</param>
    /// <param name="defaultValue">Default value if not found</param>
    /// <returns>Loaded data or default value</returns>
        public static T Load<T>(string key, T defaultValue = default)
        {
            EnsureInitialized();

            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Load key cannot be null or empty");
                return defaultValue;
            }

            try
            {
                return dataProvider.Load(key, defaultValue);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"DataManager.Load failed for key '{key}': {e.Message}");
                return defaultValue;
            }
        }

    /// <summary>
    /// Check if data exists
    /// </summary>
    /// <param name="key">Key to check</param>
    /// <returns>True if exists</returns>
        public static bool Exists(string key)
        {
            EnsureInitialized();

            if (string.IsNullOrEmpty(key))
                return false;

            try
            {
                return dataProvider.Exists(key);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"DataManager.Exists failed for key '{key}': {e.Message}");
                return false;
            }
        }

    /// <summary>
    /// Delete data by key
    /// </summary>
    /// <param name="key">Key to delete</param>
        public static void Delete(string key)
        {
            EnsureInitialized();

            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Delete key cannot be null or empty");
                return;
            }

            try
            {
                dataProvider.Delete(key);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"DataManager.Delete failed for key '{key}': {e.Message}");
            }
        }

    /// <summary>
    /// Get a list of all saved keys (only available for FileDataProvider)
    /// </summary>
    /// <returns>Array of keys</returns>
        public static string[] GetAllKeys()
        {
            EnsureInitialized();

            if (dataProvider is FileDataProvider fileProvider)
            {
                return fileProvider.GetAllKeys();
            }

            Debug.LogWarning("GetAllKeys is only supported with FileDataProvider");
            return new string[0];
        }

    /// <summary>
    /// Utility method to save game settings
    /// </summary>
    /// <param name="settings">Settings object</param>
        public static void SaveGameSettings(object settings)
        {
            Save("game_settings", settings);
        }

    /// <summary>
    /// Utility method to load game settings
    /// </summary>
    /// <typeparam name="T">Settings type</typeparam>
    /// <param name="defaultSettings">Default settings</param>
    /// <returns>Loaded settings</returns>
        public static T LoadGameSettings<T>(T defaultSettings = default)
        {
            return Load("game_settings", defaultSettings);
        }

    /// <summary>
    /// Utility method to save player progress
    /// </summary>
    /// <param name="playerData">Player data</param>
    /// <param name="slotIndex">Save slot index (default 0)</param>
        public static void SavePlayerProgress(object playerData, int slotIndex = 0)
        {
            Save($"player_progress_slot_{slotIndex}", playerData);
        }

    /// <summary>
    /// Utility method to load player progress
    /// </summary>
    /// <typeparam name="T">Player data type</typeparam>
    /// <param name="slotIndex">Save slot index (default 0)</param>
    /// <param name="defaultData">Default data</param>
    /// <returns>Loaded player data</returns>
        public static T LoadPlayerProgress<T>(int slotIndex = 0, T defaultData = default)
        {
            return Load($"player_progress_slot_{slotIndex}", defaultData);
        }

        private static void EnsureInitialized()
        {
            if (!isInitialized || dataProvider == null)
            {
                Debug.LogWarning("DataManager not properly initialized. Using default configuration.");
                InitializeDefault();
            }
        }
    }

}
