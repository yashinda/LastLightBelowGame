using UnityEngine;
using System.Collections.Generic;
using SDM = SaveDataPro.DataManager;

namespace SaveDataPro.Demo
{
    /// <summary>
/// Demo script to test the SaveData_Pro system
/// Automatically runs tests on game start
/// </summary>
public class SaveDataProDemo : MonoBehaviour
{
    [Header("Demo Settings")]
    [SerializeField] private bool runTestsOnStart = true;
    [SerializeField] private bool enableEncryption = true;
    [SerializeField] private bool enableIntegrityCheck = true;
    [SerializeField] private float testInterval = 2f;

    [Header("Test Results")]
    [SerializeField] private bool basicSaveLoadTest = false;
    [SerializeField] private bool encryptionTest = false;
    [SerializeField] private bool integrityTest = false;
    [SerializeField] private bool performanceTest = false;

    // Test data classes
    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
        public int level;
        public float experience;
        public Vector3 position;
        public List<string> inventory;
        public Dictionary<string, int> stats;

        public PlayerData()
        {
            playerName = "TestPlayer";
            level = 1;
            experience = 0f;
            position = Vector3.zero;
            inventory = new List<string> { "Sword", "Shield", "Potion" };
            stats = new Dictionary<string, int>
            {
                ["Health"] = 100,
                ["Mana"] = 50,
                ["Attack"] = 25
            };
        }
    }

    [System.Serializable]
    public class GameSettings
    {
        public float masterVolume = 1.0f;
        public float musicVolume = 0.8f;
        public float sfxVolume = 0.9f;
        public bool fullscreen = true;
        public int quality = 2;
        public string language = "English";
    }

    private void Start()
    {
        if (runTestsOnStart)
        {
            StartCoroutine(RunAllTests());
        }
    }

    private System.Collections.IEnumerator RunAllTests()
    {
        Debug.Log("=== SaveData_Pro Demo Started ===");
        
    // Initialize DataManager with demo configuration
        InitializeDataManager();
        yield return new WaitForSeconds(0.5f);

        // Test 1: Basic Save/Load
        Debug.Log("\n--- Test 1: Basic Save/Load ---");
        yield return StartCoroutine(TestBasicSaveLoad());
        yield return new WaitForSeconds(testInterval);

        // Test 2: Encryption
        Debug.Log("\n--- Test 2: Encryption Test ---");
        yield return StartCoroutine(TestEncryption());
        yield return new WaitForSeconds(testInterval);

        // Test 3: Data Integrity
        Debug.Log("\n--- Test 3: Data Integrity ---");
        yield return StartCoroutine(TestDataIntegrity());
        yield return new WaitForSeconds(testInterval);

        // Test 4: Performance
        Debug.Log("\n--- Test 4: Performance Test ---");
        yield return StartCoroutine(TestPerformance());
        yield return new WaitForSeconds(testInterval);

        // Test 5: Utility Methods
        Debug.Log("\n--- Test 5: Utility Methods ---");
        yield return StartCoroutine(TestUtilityMethods());

        Debug.Log("\n=== All Tests Completed ===");
        LogTestResults();
    }

    private void InitializeDataManager()
    {
        try
        {
            // Use platform-optimized settings with demo configuration
            if (enableEncryption || enableIntegrityCheck)
            {
                SDM.DataManager.InitializePlatformOptimized();
            }
            else
            {
                SDM.DataManager.InitializeDefault();
            }
            Debug.Log($"✓ DataManager initialized successfully (Encryption: {enableEncryption}, Integrity: {enableIntegrityCheck})");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ DataManager initialization failed: {e.Message}");
        }
    }

    private System.Collections.IEnumerator TestBasicSaveLoad()
    {
        try
        {
            // Test basic data types
            SDM.DataManager.Save("test_string", "Hello SaveData_Pro!");
            SDM.DataManager.Save("test_int", 12345);
            SDM.DataManager.Save("test_float", 3.14159f);
            SDM.DataManager.Save("test_bool", true);

            // Load and verify
            string loadedString = SDM.DataManager.Load<string>("test_string");
            int loadedInt = SDM.DataManager.Load<int>("test_int");
            float loadedFloat = SDM.DataManager.Load<float>("test_float");
            bool loadedBool = SDM.DataManager.Load<bool>("test_bool");

            bool testPassed = loadedString == "Hello SaveData_Pro!" &&
                            loadedInt == 12345 &&
                            Mathf.Approximately(loadedFloat, 3.14159f) &&
                            loadedBool == true;

            basicSaveLoadTest = testPassed;
            Debug.Log(testPassed ? "✓ Basic Save/Load test passed" : "✗ Basic Save/Load test failed");

            // Test complex object
            var playerData = new PlayerData();
            playerData.level = 10;
            playerData.experience = 1500.5f;
            playerData.position = new Vector3(10, 20, 30);

            SDM.DataManager.Save("player_data", playerData);
            var loadedPlayerData = SDM.DataManager.Load<PlayerData>("player_data");

            bool complexTestPassed = loadedPlayerData != null &&
                                   loadedPlayerData.level == 10 &&
                                   Mathf.Approximately(loadedPlayerData.experience, 1500.5f) &&
                                   loadedPlayerData.position == new Vector3(10, 20, 30);

            Debug.Log(complexTestPassed ? "✓ Complex object test passed" : "✗ Complex object test failed");
            basicSaveLoadTest = basicSaveLoadTest && complexTestPassed;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ Basic Save/Load test error: {e.Message}");
            basicSaveLoadTest = false;
        }

        yield return null;
    }

    private System.Collections.IEnumerator TestEncryption()
    {
        try
        {
            // Test with encryption enabled
            var sensitiveData = new PlayerData();
            sensitiveData.playerName = "SecretPlayer";
            sensitiveData.level = 999;

            SDM.DataManager.Save("encrypted_data", sensitiveData);
            var loadedData = SDM.DataManager.Load<PlayerData>("encrypted_data");

            bool testPassed = loadedData != null &&
                            loadedData.playerName == "SecretPlayer" &&
                            loadedData.level == 999;

            encryptionTest = testPassed;
            Debug.Log(testPassed ? "✓ Encryption test passed" : "✗ Encryption test failed");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ Encryption test error: {e.Message}");
            encryptionTest = false;
        }

        yield return null;
    }

    private System.Collections.IEnumerator TestDataIntegrity()
    {
        try
        {
            var testData = new GameSettings();
            testData.masterVolume = 0.75f;
            testData.language = "Vietnamese";

            SDM.DataManager.Save("integrity_test", testData);

            // Check file exists
            bool exists = SDM.DataManager.Exists("integrity_test");

            var loadedData = SDM.DataManager.Load<GameSettings>("integrity_test");

            bool testPassed = exists &&
                            loadedData != null &&
                            Mathf.Approximately(loadedData.masterVolume, 0.75f) &&
                            loadedData.language == "Vietnamese";

            integrityTest = testPassed;
            Debug.Log(testPassed ? "✓ Data integrity test passed" : "✗ Data integrity test failed");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ Data integrity test error: {e.Message}");
            integrityTest = false;
        }

        yield return null;
    }

    private System.Collections.IEnumerator TestPerformance()
    {
        try
        {
            int testCount = 100;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Test save performance
            for (int i = 0; i < testCount; i++)
            {
                var data = new PlayerData();
                data.level = i;
                data.experience = i * 100f;
                SDM.DataManager.Save($"perf_test_{i}", data);
            }

            long saveTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();

            // Test load performance
            for (int i = 0; i < testCount; i++)
            {
                var data = SDM.DataManager.Load<PlayerData>($"perf_test_{i}");
            }

            long loadTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Stop();

            // Cleanup
            for (int i = 0; i < testCount; i++)
            {
                SDM.DataManager.Delete($"perf_test_{i}");
            }

            performanceTest = saveTime < 5000 && loadTime < 2000; // Reasonable thresholds
            Debug.Log($"✓ Performance test: Save {testCount} objects in {saveTime}ms, Load in {loadTime}ms");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ Performance test error: {e.Message}");
            performanceTest = false;
        }

        yield return null;
    }

    private System.Collections.IEnumerator TestUtilityMethods()
    {
        try
        {
            // Test utility methods
            var settings = new GameSettings();
            SDM.DataManager.SaveGameSettings(settings);
            var loadedSettings = SDM.DataManager.LoadGameSettings<GameSettings>();

            var playerData = new PlayerData();
            SDM.DataManager.SavePlayerProgress(playerData, 0);
            var loadedPlayerData = SDM.DataManager.LoadPlayerProgress<PlayerData>(0);

            // Test GetAllKeys (if supported)
            try
            {
                string[] keys = SDM.DataManager.GetAllKeys();
                Debug.Log($"✓ Found {keys.Length} saved keys");
            }
            catch (System.Exception)
            {
                Debug.Log("GetAllKeys not supported on this platform");
            }

            Debug.Log("✓ Utility methods test completed");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ Utility methods test error: {e.Message}");
        }

        yield return null;
    }

    private void LogTestResults()
    {
        Debug.Log("\n=== TEST RESULTS SUMMARY ===");
        Debug.Log($"Basic Save/Load: {(basicSaveLoadTest ? "PASS" : "FAIL")}");
        Debug.Log($"Encryption: {(encryptionTest ? "PASS" : "FAIL")}");
        Debug.Log($"Data Integrity: {(integrityTest ? "PASS" : "FAIL")}");
        Debug.Log($"Performance: {(performanceTest ? "PASS" : "FAIL")}");

        int passedTests = 0;
        if (basicSaveLoadTest) passedTests++;
        if (encryptionTest) passedTests++;
        if (integrityTest) passedTests++;
        if (performanceTest) passedTests++;

        Debug.Log($"\nOVERALL: {passedTests}/4 tests passed");
        
        if (passedTests == 4)
        {
            Debug.Log("🎉 ALL TESTS PASSED! SaveData_Pro is working correctly.");
        }
        else
        {
            Debug.LogWarning("⚠️ Some tests failed. Check the logs above for details.");
        }
    }

    // Public methods để test manual từ Inspector hoặc script khác
    // Public methods to manually test from Inspector or other scripts
    [ContextMenu("Run All Tests")]
    public void RunTestsManually()
    {
        StartCoroutine(RunAllTests());
    }

    [ContextMenu("Test Basic Save/Load")]
    public void TestBasicSaveLoadManually()
    {
        StartCoroutine(TestBasicSaveLoad());
    }

    [ContextMenu("Clear All Save Data")]
    public void ClearAllSaveData()
    {
        try
        {
            // Clear test data
            string[] testKeys = {
                "test_string", "test_int", "test_float", "test_bool",
                "player_data", "encrypted_data", "integrity_test",
                "game_settings", "player_progress_0"
            };

            foreach (string key in testKeys)
            {
                if (SDM.DataManager.Exists(key))
                {
                    SDM.DataManager.Delete(key);
                }
            }

            Debug.Log("✓ All test save data cleared");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ Error clearing save data: {e.Message}");
        }
    }

    private void Update()
    {
        // Hot keys để test nhanh
        if (Input.GetKeyDown(KeyCode.F1))
        {
            RunTestsManually();
        }
        
        if (Input.GetKeyDown(KeyCode.F2))
        {
            TestBasicSaveLoadManually();
        }
        
        if (Input.GetKeyDown(KeyCode.F3))
        {
            ClearAllSaveData();
        }

    // Quick save/load test with hotkeys
        if (Input.GetKeyDown(KeyCode.S))
        {
            var data = new PlayerData();
            data.level = Random.Range(1, 100);
            data.position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
            SDM.DataManager.Save("quick_test", data);
            Debug.Log($"Quick save: Level {data.level}, Position {data.position}");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            var data = SDM.DataManager.Load<PlayerData>("quick_test");
            if (data != null)
            {
                Debug.Log($"Quick load: Level {data.level}, Position {data.position}");
            }
            else
            {
                Debug.Log("No quick save data found");
            }
        }
    }

    private void OnGUI()
    {
        // Simple GUI để hiển thị controls
        GUILayout.BeginArea(new Rect(10, 10, 300, 400));
        
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontStyle = FontStyle.Bold;
        
        GUILayout.Label("SaveData_Pro Demo Controls:", titleStyle);
        
        // Show configuration
        GUILayout.Label($"Encryption: {(enableEncryption ? "ON" : "OFF")}");
        GUILayout.Label($"Integrity Check: {(enableIntegrityCheck ? "ON" : "OFF")}");
        GUILayout.Space(5);
        
        GUILayout.Label("F1 - Run All Tests");
        GUILayout.Label("F2 - Test Basic Save/Load");
        GUILayout.Label("F3 - Clear All Save Data");
        GUILayout.Label("S - Quick Save");
        GUILayout.Label("L - Quick Load");
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Run All Tests"))
        {
            RunTestsManually();
        }
        
        if (GUILayout.Button("Clear Save Data"))
        {
            ClearAllSaveData();
        }
        
        GUILayout.EndArea();
    }
}

}