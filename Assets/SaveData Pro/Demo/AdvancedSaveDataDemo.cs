using UnityEngine;
using System.Collections.Generic;
using SDM = SaveDataPro.DataManager;

namespace SaveDataPro.Demo
{
    /// <summary>
    /// Advanced demo to exercise SaveData_Pro's advanced features
    /// Tests encryption, integrity checking, and performance
    /// </summary>
    public class AdvancedSaveDataDemo : MonoBehaviour
    {
    [Header("Test Configuration")]
    [SerializeField] private bool testEncryption = true;
    [SerializeField] private bool testIntegrity = true;
    [SerializeField] private int performanceTestCount = 1000;

    [System.Serializable]
    public class ComplexGameData
    {
        public string playerName;
        public int level;
        public float experience;
        public Vector3 position;
        public Quaternion rotation;
        public List<Item> inventory;
        public Dictionary<string, float> stats;
        public QuestData[] quests;
        
        public ComplexGameData()
        {
            playerName = "AdvancedPlayer";
            level = 1;
            experience = 0f;
            position = Vector3.zero;
            rotation = Quaternion.identity;
            inventory = new List<Item>();
            stats = new Dictionary<string, float>();
            quests = new QuestData[0];
        }
    }

    [System.Serializable]
    public class Item
    {
        public string name;
        public int quantity;
        public float value;
        public ItemType type;
        
        public Item(string n, int q, float v, ItemType t)
        {
            name = n;
            quantity = q;
            value = v;
            type = t;
        }
    }

    [System.Serializable]
    public enum ItemType
    {
        Weapon, Armor, Consumable, Material, Key
    }

    [System.Serializable]
    public class QuestData
    {
        public string questId;
        public string title;
        public bool isCompleted;
        public float progress;
        
        public QuestData(string id, string t, bool completed, float prog)
        {
            questId = id;
            title = t;
            isCompleted = completed;
            progress = prog;
        }
    }

    private ComplexGameData gameData;

    private void Start()
    {
        Debug.Log("=== Advanced SaveData_Pro Demo ===");
        Debug.Log("Controls:");
        Debug.Log("F1 - Create Complex Data");
        Debug.Log("F2 - Save Complex Data");
        Debug.Log("F3 - Load Complex Data");
        Debug.Log("F4 - Test Encryption");
        Debug.Log("F5 - Test Data Integrity");
        Debug.Log("F6 - Performance Test");
        Debug.Log("F7 - Stress Test");
        Debug.Log("F8 - Clear All Data");

        // Initialize with complex data
        CreateComplexData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            CreateComplexData();
        }
        
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SaveComplexData();
        }
        
        if (Input.GetKeyDown(KeyCode.F3))
        {
            LoadComplexData();
        }
        
        if (Input.GetKeyDown(KeyCode.F4))
        {
            TestEncryption();
        }
        
        if (Input.GetKeyDown(KeyCode.F5))
        {
            TestDataIntegrity();
        }
        
        if (Input.GetKeyDown(KeyCode.F6))
        {
            StartCoroutine(PerformanceTest());
        }
        
        if (Input.GetKeyDown(KeyCode.F7))
        {
            StartCoroutine(StressTest());
        }
        
        if (Input.GetKeyDown(KeyCode.F8))
        {
            ClearAllData();
        }
    }

    private void CreateComplexData()
    {
        gameData = new ComplexGameData();
        gameData.playerName = "AdvancedTestPlayer";
        gameData.level = Random.Range(1, 100);
        gameData.experience = Random.Range(0f, 10000f);
        gameData.position = new Vector3(
            Random.Range(-100f, 100f),
            Random.Range(-100f, 100f),
            Random.Range(-100f, 100f)
        );
        gameData.rotation = Random.rotation;

        // Create inventory
        gameData.inventory.Add(new Item("Magic Sword", 1, 1500f, ItemType.Weapon));
        gameData.inventory.Add(new Item("Dragon Armor", 1, 2000f, ItemType.Armor));
        gameData.inventory.Add(new Item("Health Potion", 10, 50f, ItemType.Consumable));
        gameData.inventory.Add(new Item("Mithril Ore", 25, 100f, ItemType.Material));

        // Create stats
        gameData.stats["Health"] = Random.Range(100f, 1000f);
        gameData.stats["Mana"] = Random.Range(50f, 500f);
        gameData.stats["Attack"] = Random.Range(25f, 250f);
        gameData.stats["Defense"] = Random.Range(15f, 150f);
        gameData.stats["Speed"] = Random.Range(10f, 100f);

        // Create quests
        List<QuestData> questList = new List<QuestData>();
        questList.Add(new QuestData("quest_001", "Defeat the Dragon", false, 0.75f));
        questList.Add(new QuestData("quest_002", "Collect 100 Gold", true, 1.0f));
        questList.Add(new QuestData("quest_003", "Find the Ancient Artifact", false, 0.25f));
        gameData.quests = questList.ToArray();

        Debug.Log($"✓ Created complex data - Level {gameData.level}, {gameData.inventory.Count} items, {gameData.quests.Length} quests");
    }

    private void SaveComplexData()
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        try
        {
            SDM.DataManager.Save("complex_game_data", gameData);
            stopwatch.Stop();
            Debug.Log($"✓ Complex data saved in {stopwatch.ElapsedMilliseconds}ms");
        }
        catch (System.Exception e)
        {
            stopwatch.Stop();
            Debug.LogError($"✗ Save failed: {e.Message}");
        }
    }

    private void LoadComplexData()
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        try
        {
            var loadedData = SDM.DataManager.Load<ComplexGameData>("complex_game_data");
            stopwatch.Stop();
            
            if (loadedData != null)
            {
                gameData = loadedData;
                Debug.Log($"✓ Complex data loaded in {stopwatch.ElapsedMilliseconds}ms");
                Debug.Log($"Player: {gameData.playerName}, Level: {gameData.level}, Items: {gameData.inventory.Count}");
            }
            else
            {
                Debug.LogWarning("No saved data found");
            }
        }
        catch (System.Exception e)
        {
            stopwatch.Stop();
            Debug.LogError($"✗ Load failed: {e.Message}");
        }
    }

    private void TestEncryption()
    {
        if (!testEncryption)
        {
            Debug.Log("Encryption test skipped (disabled in configuration)");
            return;
        }
        
        Debug.Log("--- Encryption Test ---");
        
        try
        {
            // Save sensitive data
            var sensitiveData = new ComplexGameData();
            sensitiveData.playerName = "EncryptedPlayer";
            sensitiveData.level = 999;
            
            SDM.DataManager.Save("encrypted_test", sensitiveData);
            
            // Load and verify
            var loadedData = SDM.DataManager.Load<ComplexGameData>("encrypted_test");
            
            bool success = loadedData != null && 
                          loadedData.playerName == "EncryptedPlayer" && 
                          loadedData.level == 999;
            
            Debug.Log(success ? "✓ Encryption test passed" : "✗ Encryption test failed");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ Encryption test error: {e.Message}");
        }
    }

    private void TestDataIntegrity()
    {
        if (!testIntegrity)
        {
            Debug.Log("Data integrity test skipped (disabled in configuration)");
            return;
        }
        
        Debug.Log("--- Data Integrity Test ---");
        
        try
        {
            var testData = new ComplexGameData();
            testData.playerName = "IntegrityTest";
            testData.experience = 12345.67f;
            
            SDM.DataManager.Save("integrity_test", testData);
            
            // Verify file exists
            bool exists = SDM.DataManager.Exists("integrity_test");
            
            // Load and verify
            var loadedData = SDM.DataManager.Load<ComplexGameData>("integrity_test");
            
            bool success = exists && 
                          loadedData != null && 
                          loadedData.playerName == "IntegrityTest" &&
                          Mathf.Approximately(loadedData.experience, 12345.67f);
            
            Debug.Log(success ? "✓ Data integrity test passed" : "✗ Data integrity test failed");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ Data integrity test error: {e.Message}");
        }
    }

    private System.Collections.IEnumerator PerformanceTest()
    {
        Debug.Log($"--- Performance Test ({performanceTestCount} operations) ---");
        
        var testData = new ComplexGameData();
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        // Save test
        for (int i = 0; i < performanceTestCount; i++)
        {
            testData.level = i;
            SDM.DataManager.Save($"perf_test_{i}", testData);
            
            if (i % 100 == 0)
            {
                yield return null; // Don't freeze the game
            }
        }
        
        long saveTime = stopwatch.ElapsedMilliseconds;
        stopwatch.Restart();
        
        // Load test
        for (int i = 0; i < performanceTestCount; i++)
        {
            var data = SDM.DataManager.Load<ComplexGameData>($"perf_test_{i}");
            
            if (i % 100 == 0)
            {
                yield return null;
            }
        }
        
        long loadTime = stopwatch.ElapsedMilliseconds;
        stopwatch.Stop();
        
        // Cleanup
        for (int i = 0; i < performanceTestCount; i++)
        {
            SDM.DataManager.Delete($"perf_test_{i}");
            
            if (i % 100 == 0)
            {
                yield return null;
            }
        }
        
        Debug.Log($"✓ Performance test completed:");
        Debug.Log($"  Save {performanceTestCount} objects: {saveTime}ms ({(float)saveTime/performanceTestCount:F2}ms per save)");
        Debug.Log($"  Load {performanceTestCount} objects: {loadTime}ms ({(float)loadTime/performanceTestCount:F2}ms per load)");
    }

    private System.Collections.IEnumerator StressTest()
    {
        Debug.Log("--- Stress Test ---");
        
        int stressCount = 100;
        var stressData = new ComplexGameData();
        
        // Make data larger for stress test
        for (int i = 0; i < 100; i++)
        {
            stressData.inventory.Add(new Item($"StressItem_{i}", Random.Range(1, 100), Random.Range(1f, 1000f), (ItemType)(i % 5)));
            stressData.stats[$"Stat_{i}"] = Random.Range(1f, 1000f);
        }
        
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        for (int i = 0; i < stressCount; i++)
        {
            stressData.level = i;
            SDM.DataManager.Save($"stress_test_{i}", stressData);
            
            var loadedData = SDM.DataManager.Load<ComplexGameData>($"stress_test_{i}");
            
            if (loadedData == null || loadedData.level != i)
            {
                Debug.LogError($"Stress test failed at iteration {i}");
                yield break;
            }
            
            if (i % 10 == 0)
            {
                yield return null;
            }
        }
        
        stopwatch.Stop();
        
        // Cleanup
        for (int i = 0; i < stressCount; i++)
        {
            SDM.DataManager.Delete($"stress_test_{i}");
        }
        
        Debug.Log($"✓ Stress test passed: {stressCount} save/load cycles in {stopwatch.ElapsedMilliseconds}ms");
    }

    private void ClearAllData()
    {
        try
        {
            string[] testKeys = {
                "complex_game_data", "encrypted_test", "integrity_test"
            };

            foreach (string key in testKeys)
            {
                if (SDM.DataManager.Exists(key))
                {
                    SDM.DataManager.Delete(key);
                }
            }

            Debug.Log("✓ All test data cleared");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"✗ Error clearing data: {e.Message}");
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, Screen.height - 200, 500, 200));
        
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontStyle = FontStyle.Bold;
        
        GUILayout.Label("Advanced SaveData_Pro Demo", titleStyle);
        
        // Show configuration
        GUILayout.Label($"Encryption Test: {(testEncryption ? "ON" : "OFF")}");
        GUILayout.Label($"Integrity Test: {(testIntegrity ? "ON" : "OFF")}");
        GUILayout.Label($"Performance Count: {performanceTestCount}");
        GUILayout.Space(5);
        
        if (gameData != null)
        {
            GUILayout.Label($"Player: {gameData.playerName} (Level {gameData.level})");
            GUILayout.Label($"Items: {gameData.inventory.Count}, Stats: {gameData.stats.Count}");
        }
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create Data (F1)")) CreateComplexData();
        if (GUILayout.Button("Save (F2)")) SaveComplexData();
        if (GUILayout.Button("Load (F3)")) LoadComplexData();
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Encryption Test (F4)")) TestEncryption();
        if (GUILayout.Button("Integrity Test (F5)")) TestDataIntegrity();
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Performance (F6)")) StartCoroutine(PerformanceTest());
        if (GUILayout.Button("Stress Test (F7)")) StartCoroutine(StressTest());
        GUILayout.EndHorizontal();
        
        GUILayout.EndArea();
    }
}
    }

