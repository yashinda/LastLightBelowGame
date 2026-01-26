using UnityEngine;
using SDM = SaveDataPro.DataManager;

namespace SaveDataPro.Demo
{
    /// <summary>
    /// Platform-specific demo to test platform optimization features
    /// </summary>
    public class PlatformSpecificDemo : MonoBehaviour
    {
    [Header("Platform Info")]
    [SerializeField] private string currentPlatform;
    [SerializeField] private string saveDirectory;
    [SerializeField] private bool encryptionRecommended;
    [SerializeField] private bool autoSaveRecommended;

    private void Start()
    {
        Debug.Log("=== Platform-Specific SaveData_Pro Demo ===");
        
        // Get platform information
        UpdatePlatformInfo();
        
        // Initialize with platform-optimized settings
        SDM.PlatformSupport.InitializePlatformOptimized();
        
        LogPlatformInfo();
        
        Debug.Log("Controls:");
        Debug.Log("P - Show Platform Info");
        Debug.Log("O - Test Platform Optimization");
        Debug.Log("C - Test Custom Config");
        Debug.Log("R - Reset to Platform Default");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShowPlatformInfo();
        }
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            TestPlatformOptimization();
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            TestCustomConfig();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetToPlatformDefault();
        }
    }

    private void UpdatePlatformInfo()
    {
#if UNITY_WEBGL
        currentPlatform = "WebGL";
#elif UNITY_ANDROID
        currentPlatform = "Android";
#elif UNITY_IOS
        currentPlatform = "iOS";
#elif UNITY_STANDALONE_WIN
        currentPlatform = "Windows Standalone";
#elif UNITY_STANDALONE_OSX
        currentPlatform = "macOS Standalone";
#elif UNITY_STANDALONE_LINUX
        currentPlatform = "Linux Standalone";
#else
        currentPlatform = "Unknown Platform";
#endif

        saveDirectory = SDM.PlatformSupport.GetOptimizedSaveDirectory();
        encryptionRecommended = SDM.PlatformSupport.IsEncryptionRecommended();
        autoSaveRecommended = SDM.PlatformSupport.IsAutoSaveRecommended();
    }

    private void LogPlatformInfo()
    {
        Debug.Log($"Platform: {currentPlatform}");
        Debug.Log($"Save Directory: {saveDirectory}");
        Debug.Log($"Encryption Recommended: {encryptionRecommended}");
        Debug.Log($"Auto-Save Recommended: {autoSaveRecommended}");
        Debug.Log($"Recommended Serialization: {SDM.PlatformSupport.GetRecommendedSerialization()}");
        Debug.Log($"Recommended Cache Size: {SDM.PlatformSupport.GetRecommendedCacheSize()}MB");

        if (autoSaveRecommended)
        {
            Debug.Log($"Auto-Save Interval: {SDM.PlatformSupport.GetRecommendedAutoSaveInterval()}s");
        }
    }

    private void ShowPlatformInfo()
    {
        Debug.Log("=== Current Platform Information ===");
        LogPlatformInfo();
        
        // Additional platform-specific info
        Debug.Log($"Persistent Data Path: {Application.persistentDataPath}");
        Debug.Log($"Platform File Extension: {SDM.PlatformSupport.GetPlatformFileExtension()}");
        Debug.Log($"Supports Custom Save Directory: {SDM.PlatformSupport.SupportsCustomSaveDirectory()}");
        
        // Test platform capabilities
        TestPlatformCapabilities();
    }

    private void TestPlatformCapabilities()
    {
        Debug.Log("--- Platform Capabilities Test ---");
        
        try
        {
            // Test basic save/load
            var testData = new PlatformTestData();
            testData.platformName = currentPlatform;
            testData.timestamp = System.DateTime.Now.ToBinary();

            SDM.DataManager.Save("platform_test", testData);
            var loadedData = SDM.DataManager.Load<PlatformTestData>("platform_test");
    
            bool basicTest = loadedData != null && loadedData.platformName == currentPlatform;
            Debug.Log(basicTest ? "✓ Basic platform test passed" : "✗ Basic platform test failed");
            
            // Test GetAllKeys if supported
            try
            {
                string[] keys = SDM.DataManager.GetAllKeys();
                Debug.Log($"✓ GetAllKeys supported: Found {keys.Length} keys");
            }
            catch (System.Exception)
            {
                Debug.Log("GetAllKeys not supported on this platform");
            }
            
            // Cleanup
            SDM.DataManager.Delete("platform_test");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ Platform capabilities test failed: {e.Message}");
        }
    }

    private void TestPlatformOptimization()
    {
        Debug.Log("--- Platform Optimization Test ---");
        
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        try
        {
            // Create platform-appropriate test data
            var testData = CreatePlatformTestData();
            
            // Test save performance
            SDM.DataManager.Save("optimization_test", testData);
            long saveTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();
            
            // Test load performance
            var loadedData = SDM.DataManager.Load<PlatformTestData>("optimization_test");
            long loadTime = stopwatch.ElapsedMilliseconds;
            
            // Verify data integrity
            bool dataIntact = loadedData != null && 
                            loadedData.platformName == testData.platformName &&
                            loadedData.testData.Length == testData.testData.Length;
            
            Debug.Log($"✓ Optimization test results:");
            Debug.Log($"  Save time: {saveTime}ms");
            Debug.Log($"  Load time: {loadTime}ms");
            Debug.Log($"  Data integrity: {(dataIntact ? "OK" : "FAILED")}");
            
            // Platform-specific performance thresholds
            int expectedSaveTime = GetExpectedSaveTime();
            int expectedLoadTime = GetExpectedLoadTime();
            
            if (saveTime <= expectedSaveTime && loadTime <= expectedLoadTime)
            {
                Debug.Log("✓ Performance within expected range for this platform");
            }
            else
            {
                Debug.LogWarning($"⚠️ Performance outside expected range (Save: {expectedSaveTime}ms, Load: {expectedLoadTime}ms)");
            }
            
            // Cleanup
            SDM.DataManager.Delete("optimization_test");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ Platform optimization test failed: {e.Message}");
        }
    }

    private void TestCustomConfig()
    {
        Debug.Log("--- Custom Config Test ---");
        
        try
        {
            // Get platform-optimized config
            var config = SDM.PlatformSupport.GetPlatformOptimizedConfig();
            
            Debug.Log($"Platform Config:");
            Debug.Log($"  Encryption Key Size: {config.aesKeySize} bytes");
            Debug.Log($"  Default Serialization: {config.defaultSerializationType}");
            Debug.Log($"  Enable Auto-Save: {config.enableAutoSave}");
            Debug.Log($"  Auto-Save Interval: {config.autoSaveInterval}s");
            Debug.Log($"  Max Cache Size: {config.maxCacheSizeMB}MB");
            Debug.Log($"  File Extension: {config.fileExtension}");
            
            // Test with custom config
            var testData = new PlatformTestData();
            testData.configTest = true;

            SDM.DataManager.Save("config_test", testData);
            var loadedData = SDM.DataManager.Load<PlatformTestData>("config_test");

            bool success = loadedData != null && loadedData.configTest;
            Debug.Log(success ? "✓ Custom config test passed" : "✗ Custom config test failed");

            SDM.DataManager.Delete("config_test");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ Custom config test failed: {e.Message}");
        }
    }

    private void ResetToPlatformDefault()
    {
        Debug.Log("--- Resetting to Platform Default ---");
        
        try
        {
            // Reset config to platform default
            SDM.SaveDataProConfig.ResetToDefault();

            // Reinitialize DataManager
            SDM.PlatformSupport.InitializePlatformOptimized();

            // Update platform info
            UpdatePlatformInfo();
            
            Debug.Log("✓ Reset to platform default completed");
            LogPlatformInfo();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ Reset to platform default failed: {e.Message}");
        }
    }

    private PlatformTestData CreatePlatformTestData()
    {
        var data = new PlatformTestData();
        data.platformName = currentPlatform;
        data.timestamp = System.DateTime.Now.ToBinary();
        data.configTest = false;
        
        // Create platform-appropriate amount of test data
        int dataSize = GetPlatformDataSize();
        data.testData = new float[dataSize];
        
        for (int i = 0; i < dataSize; i++)
        {
            data.testData[i] = Random.Range(0f, 1000f);
        }
        
        return data;
    }

    private int GetPlatformDataSize()
    {
#if UNITY_WEBGL
        return 100; // Smaller data for web
#elif UNITY_ANDROID || UNITY_IOS
        return 500; // Moderate data for mobile
#elif UNITY_STANDALONE
        return 1000; // Larger data for desktop
#else
        return 250; // Conservative default
#endif
    }

    private int GetExpectedSaveTime()
    {
#if UNITY_WEBGL
        return 100; // More lenient for web
#elif UNITY_ANDROID || UNITY_IOS
        return 50; // Mobile should be reasonably fast
#elif UNITY_STANDALONE
        return 25; // Desktop should be fast
#else
        return 75; // Conservative default
#endif
    }

    private int GetExpectedLoadTime()
    {
#if UNITY_WEBGL
        return 50; // More lenient for web
#elif UNITY_ANDROID || UNITY_IOS
        return 25; // Mobile should be reasonably fast
#elif UNITY_STANDALONE
        return 10; // Desktop should be very fast
#else
        return 35; // Conservative default
#endif
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 300, 10, 290, 250));
        
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontStyle = FontStyle.Bold;
        
        GUILayout.Label("Platform-Specific Demo", titleStyle);
        GUILayout.Label($"Platform: {currentPlatform}");
        GUILayout.Label($"Encryption: {(encryptionRecommended ? "ON" : "OFF")}");
        GUILayout.Label($"Auto-Save: {(autoSaveRecommended ? "ON" : "OFF")}");
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Platform Info (P)"))
        {
            ShowPlatformInfo();
        }
        
        if (GUILayout.Button("Test Optimization (O)"))
        {
            TestPlatformOptimization();
        }
        
        if (GUILayout.Button("Test Config (C)"))
        {
            TestCustomConfig();
        }
        
        if (GUILayout.Button("Reset Default (R)"))
        {
            ResetToPlatformDefault();
        }
        
        GUILayout.EndArea();
    }

    [System.Serializable]
    public class PlatformTestData
    {
        public string platformName;
        public long timestamp;
        public bool configTest;
        public float[] testData;
    }
}
}
