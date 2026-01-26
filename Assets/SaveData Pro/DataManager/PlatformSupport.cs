using UnityEngine;
using System.IO;
using SaveDataPro.DataManager;

namespace SaveDataPro.DataManager
{
/// <summary>
/// Platform-specific support utilities for SaveData_Pro
/// Handles cross-platform compatibility and optimizations
/// </summary>
public static class PlatformSupport
{
    /// <summary>
    /// Get platform-optimized save directory
    /// </summary>
    public static string GetOptimizedSaveDirectory()
    {
        string baseDir = Application.persistentDataPath;
        
#if UNITY_WEBGL
        // WebGL has limited storage, use shorter paths
        return Path.Combine(baseDir, "SD");
#elif UNITY_ANDROID || UNITY_IOS
        // Mobile platforms - standard path
        return Path.Combine(baseDir, "SaveData_Pro");
#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
        // Desktop platforms - can use more descriptive paths
        return Path.Combine(baseDir, "SaveData_Pro", "GameData");
#else
        // Default fallback
        return Path.Combine(baseDir, "SaveData_Pro");
#endif
    }

    /// <summary>
    /// Get platform-specific file extension
    /// </summary>
    public static string GetPlatformFileExtension()
    {
#if UNITY_WEBGL
        return ".dat"; // Shorter extension for web
#else
        return ".sav"; // Standard extension
#endif
    }

    /// <summary>
    /// Check if encryption is recommended for current platform
    /// </summary>
    public static bool IsEncryptionRecommended()
    {
#if UNITY_WEBGL
        return false; // Performance impact on web
#elif UNITY_ANDROID || UNITY_IOS
        return true; // Mobile security important
#elif UNITY_STANDALONE
        return true; // Desktop can handle encryption
#else
        return true; // Default to secure
#endif
    }

    /// <summary>
    /// Get platform-specific serialization recommendation
    /// </summary>
    public static SerializationType GetRecommendedSerialization()
    {
#if UNITY_WEBGL
        return SerializationType.OptimizedJson; // Performance critical
#elif UNITY_ANDROID || UNITY_IOS
        return SerializationType.Json; // Balance of features and performance
#elif UNITY_STANDALONE
        return SerializationType.Json; // Full features available
#else
        return SerializationType.Json; // Safe default
#endif
    }

    /// <summary>
    /// Get maximum recommended cache size for current platform (MB)
    /// </summary>
    public static int GetRecommendedCacheSize()
    {
#if UNITY_WEBGL
        return 10; // Limited memory on web
#elif UNITY_ANDROID || UNITY_IOS
        return 25; // Moderate cache for mobile
#elif UNITY_STANDALONE
        return 100; // More cache available on desktop
#else
        return 50; // Conservative default
#endif
    }

    /// <summary>
    /// Check if auto-save is recommended for current platform
    /// </summary>
    public static bool IsAutoSaveRecommended()
    {
#if UNITY_WEBGL
        return false; // May cause performance issues
#elif UNITY_ANDROID || UNITY_IOS
        return true; // Important for mobile apps (backgrounding)
#elif UNITY_STANDALONE
        return true; // Good UX on desktop
#else
        return true; // Generally recommended
#endif
    }

    /// <summary>
    /// Get platform-specific auto-save interval (seconds)
    /// </summary>
    public static float GetRecommendedAutoSaveInterval()
    {
#if UNITY_WEBGL
        return 600f; // 10 minutes - less frequent
#elif UNITY_ANDROID || UNITY_IOS
        return 180f; // 3 minutes - frequent due to backgrounding
#elif UNITY_STANDALONE
        return 300f; // 5 minutes - standard
#else
        return 300f; // 5 minutes default
#endif
    }

    /// <summary>
    /// Check if platform supports custom save directories
    /// </summary>
    public static bool SupportsCustomSaveDirectory()
    {
#if UNITY_WEBGL
        return false; // Limited file system access
#elif UNITY_ANDROID
        return false; // Restricted file system
#elif UNITY_IOS
        return false; // App sandbox restrictions
#elif UNITY_STANDALONE
        return true; // Full file system access
#else
        return false; // Conservative default
#endif
    }

    /// <summary>
    /// Get platform-specific performance configuration
    /// </summary>
    public static SaveDataProConfig GetPlatformOptimizedConfig()
    {
        var config = new SaveDataProConfig();
        
        // Apply platform-specific settings
        config.enableIntegrityCheck = IsEncryptionRecommended();
        config.defaultSerializationType = GetRecommendedSerialization();
        config.maxCacheSizeMB = GetRecommendedCacheSize();
        config.enableAutoSave = IsAutoSaveRecommended();
        config.autoSaveInterval = GetRecommendedAutoSaveInterval();
        config.fileExtension = GetPlatformFileExtension();
        
        // Platform-specific encryption key
        config.defaultEncryptionKey = $"SaveData_Pro_{Application.platform}_{System.DateTime.Now.Year}";
        
        // Web-specific optimizations
#if UNITY_WEBGL
        config.useOptimizedJson = true;
        config.enableVerboseLogging = false; // Reduce console spam
        config.enableAutoBackup = false; // Save storage space
#endif

        // Mobile-specific optimizations
#if UNITY_ANDROID || UNITY_IOS
        config.maxBackupCount = 2; // Limited storage
        config.enableVerboseLogging = false; // Save performance
#endif

        // Desktop-specific settings
#if UNITY_STANDALONE
        config.maxBackupCount = 5; // More storage available
        config.enableVerboseLogging = true; // Full debugging
#endif

        return config;
    }

    /// <summary>
    /// Initialize DataManager with platform-optimized settings
    /// </summary>
    public static void InitializePlatformOptimized()
    {
        var config = GetPlatformOptimizedConfig();
        
        // Choose serializer based on platform
        ISerializationOption serializer;
        if (config.useOptimizedJson || config.defaultSerializationType == SerializationType.OptimizedJson)
        {
            serializer = new OptimizedJsonSerializerOption();
        }
        else
        {
            serializer = new JsonSerializerOption();
        }
        
        // Choose encryption based on platform
        IEncryptionOption encryption = null;
        if (IsEncryptionRecommended())
        {
            encryption = new AesEncryptionOption(config.defaultEncryptionKey);
        }
        
        // Create provider with platform settings
        var provider = new FileDataProvider(serializer, encryption, config.enableIntegrityCheck);
        
        // Initialize DataManager
        DataManager.Initialize(provider);
        
        Debug.Log($"SaveData_Pro initialized for platform: {Application.platform}");
        Debug.Log($"Encryption: {(encryption != null ? "Enabled" : "Disabled")}");
        Debug.Log($"Serialization: {config.defaultSerializationType}");
        Debug.Log($"Cache Size: {config.maxCacheSizeMB}MB");
    }

    /// <summary>
    /// Log platform-specific information
    /// </summary>
    public static void LogPlatformInfo()
    {
        Debug.Log("=== SaveData_Pro Platform Information ===");
        Debug.Log($"Platform: {Application.platform}");
        Debug.Log($"Persistent Data Path: {Application.persistentDataPath}");
        Debug.Log($"Recommended Save Directory: {GetOptimizedSaveDirectory()}");
        Debug.Log($"Supports Custom Directories: {SupportsCustomSaveDirectory()}");
        Debug.Log($"Encryption Recommended: {IsEncryptionRecommended()}");
        Debug.Log($"Recommended Serialization: {GetRecommendedSerialization()}");
        Debug.Log($"Recommended Cache Size: {GetRecommendedCacheSize()}MB");
        Debug.Log($"Auto-Save Recommended: {IsAutoSaveRecommended()}");
        if (IsAutoSaveRecommended())
        {
            Debug.Log($"Recommended Auto-Save Interval: {GetRecommendedAutoSaveInterval()}s");
        }
    }
}

}