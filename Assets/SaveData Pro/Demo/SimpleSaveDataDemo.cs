using UnityEngine;
using SDM = SaveDataPro.DataManager;

namespace SaveDataPro.Demo
{
    /// <summary>
    /// Simple demo script to test SaveData_Pro with hotkeys
    /// No UI required, just attach to a GameObject in the scene
    /// </summary>
    public class SimpleSaveDataDemo : MonoBehaviour
    {
        [System.Serializable]
        public class SimplePlayerData
        {
            public string name;
            public int score;
            public float playTime;
            public Vector3 lastPosition;

            public SimplePlayerData()
            {
                name = "Player";
                score = 0;
                playTime = 0f;
                lastPosition = Vector3.zero;
            }
        }

        private SimplePlayerData currentData;

        private void Start()
        {
            // Initialize DataManager
            SDM.DataManager.InitializePlatformOptimized();

            // Load existing data or create default
            currentData = SDM.DataManager.Load<SimplePlayerData>("player_data", new SimplePlayerData());

            Debug.Log("=== Simple SaveData_Pro Demo ===");
            Debug.Log("Controls:");
            Debug.Log("1 - Save Data");
            Debug.Log("2 - Load Data");
            Debug.Log("3 - Add Score");
            Debug.Log("4 - Update Position");
            Debug.Log("5 - Reset Data");
            Debug.Log("6 - Show Current Data");

            ShowCurrentData();

            // Register for auto save if enabled in config
            if (SDM.SaveDataProConfig.Instance.enableAutoSave)
            {
                SDM.AutoSaveManager.Instance.Register(currentData, "player_data");
                Debug.Log($"[AutoSave] Registered player_data for auto save (interval: {SDM.SaveDataProConfig.Instance.autoSaveInterval}s)");
            }
        }

        private void Update()
        {
            // Save Data
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SaveData();
            }

            // Load Data
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                LoadData();
            }

            // Add Score
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                AddScore();
            }

            // Update Position
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                UpdatePosition();
            }

            // Reset Data
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                ResetData();
            }

            // Show Current Data
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                ShowCurrentData();
            }

            // Update play time
            currentData.playTime += Time.deltaTime;
        }

        private void SaveData()
        {
            try
            {
                SDM.DataManager.Save("player_data", currentData);
                Debug.Log("✓ Data saved successfully!");
                ShowCurrentData();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"✗ Save failed: {e.Message}");
            }
        }

        private void LoadData()
        {
            try
            {
                currentData = SDM.DataManager.Load<SimplePlayerData>("player_data", new SimplePlayerData());
                Debug.Log("✓ Data loaded successfully!");
                ShowCurrentData();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"✗ Load failed: {e.Message}");
            }
        }

        private void AddScore()
        {
            currentData.score += Random.Range(10, 100);
            Debug.Log($"Score increased! New score: {currentData.score}");
        }

        private void UpdatePosition()
        {
            currentData.lastPosition = new Vector3(
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f)
            );
            Debug.Log($"Position updated! New position: {currentData.lastPosition}");
        }

        private void ResetData()
        {
            currentData = new SimplePlayerData();
            Debug.Log("Data reset to default values!");
            ShowCurrentData();
        }

        private void ShowCurrentData()
        {
            Debug.Log("=== Current Data ===");
            Debug.Log($"Name: {currentData.name}");
            Debug.Log($"Score: {currentData.score}");
            Debug.Log($"Play Time: {currentData.playTime:F1}s");
            Debug.Log($"Last Position: {currentData.lastPosition}");
        }

        private void OnGUI()
        {
            // Simple on-screen display
            GUILayout.BeginArea(new Rect(10, 10, 400, 300));

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 14;

            GUILayout.Label("Simple SaveData_Pro Demo", style);
            GUILayout.Space(10);

            GUILayout.Label($"Name: {currentData.name}");
            GUILayout.Label($"Score: {currentData.score}");
            GUILayout.Label($"Play Time: {currentData.playTime:F1}s");
            GUILayout.Label($"Position: {currentData.lastPosition}");

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save (1)"))
            {
                SaveData();
            }
            if (GUILayout.Button("Load (2)"))
            {
                LoadData();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Score (3)"))
            {
                AddScore();
            }
            if (GUILayout.Button("Update Pos (4)"))
            {
                UpdatePosition();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset (5)"))
            {
                ResetData();
            }
            if (GUILayout.Button("Show Data (6)"))
            {
                ShowCurrentData();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }
    }
}
