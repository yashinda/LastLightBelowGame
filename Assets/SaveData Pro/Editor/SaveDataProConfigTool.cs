namespace SaveDataPro.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System.IO;
    using SaveDataPro.DataManager;

    /// <summary>
    /// SaveData Pro configuration tool for the Unity Editor.
    /// Allows adjusting settings without writing code.
    /// </summary>
    public class SaveDataProConfigTool : EditorWindow
    {
        private SaveDataProConfig config;
        private Vector2 scrollPosition;
        private bool encryptionFoldout = true;
        private bool integrityFoldout = true;
        private bool serializationFoldout = true;
        private bool fileSystemFoldout = true;
        private bool performanceFoldout = true;
        private bool debugFoldout = true;

        [MenuItem("Tools/SaveData Pro/Configuration")]
        public static void ShowWindow()
        {
            SaveDataProConfigTool window = GetWindow<SaveDataProConfigTool>("SaveData Pro Config");
            window.minSize = new Vector2(400, 600);
            window.Show();
        }

        private void OnEnable()
        {
            LoadConfig();
        }

        private void LoadConfig()
        {
            SaveDataProConfig.LoadConfig();
            config = SaveDataProConfig.Instance;
        }

        private void OnGUI()
        {
            if (config == null)
            {
                LoadConfig();
                return;
            }

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            // Header
            GUILayout.Space(10);
            EditorGUILayout.LabelField("SaveData Pro Configuration", EditorStyles.boldLabel);
            GUILayout.Space(10);

            // Encryption Settings
            encryptionFoldout = EditorGUILayout.Foldout(encryptionFoldout, "Encryption Settings", true);
            if (encryptionFoldout)
            {
                EditorGUI.indentLevel++;

                config.aesKeySize = EditorGUILayout.IntPopup(
                    "AES Key Size (bytes)",
                    config.aesKeySize,
                    new string[] { "128-bit (16 bytes)", "192-bit (24 bytes)", "256-bit (32 bytes)" },
                    new int[] { 16, 24, 32 }
                );

                EditorGUILayout.LabelField("AES IV Size", config.aesIvSize + " bytes (Fixed)");

                config.defaultEncryptionKey = EditorGUILayout.TextField(
                    "Default Encryption Key",
                    config.defaultEncryptionKey
                );

                EditorGUI.indentLevel--;
            }

            GUILayout.Space(5);

            // Data Integrity
            integrityFoldout = EditorGUILayout.Foldout(integrityFoldout, "Data Integrity", true);
            if (integrityFoldout)
            {
                EditorGUI.indentLevel++;

                config.secretSalt = EditorGUILayout.TextField("Secret Salt", config.secretSalt);
                config.enableIntegrityCheck = EditorGUILayout.Toggle("Enable Integrity Check", config.enableIntegrityCheck);

                EditorGUI.indentLevel--;
            }

            GUILayout.Space(5);

            // Serialization
            serializationFoldout = EditorGUILayout.Foldout(serializationFoldout, "Serialization", true);
            if (serializationFoldout)
            {
                EditorGUI.indentLevel++;

                config.defaultSerializationType = (SerializationType)EditorGUILayout.EnumPopup(
                    "Default Serialization",
                    config.defaultSerializationType
                );

                config.useOptimizedJson = EditorGUILayout.Toggle("Use Optimized JSON", config.useOptimizedJson);

                EditorGUI.indentLevel--;
            }

            GUILayout.Space(5);

            // File System
            fileSystemFoldout = EditorGUILayout.Foldout(fileSystemFoldout, "File System", true);
            if (fileSystemFoldout)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.LabelField("Custom Save Directory:");
                config.customSaveDirectory = EditorGUILayout.TextField(config.customSaveDirectory);

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Browse", GUILayout.Width(60)))
                {
                    string path = EditorUtility.OpenFolderPanel("Select Save Directory", "", "");
                    if (!string.IsNullOrEmpty(path))
                    {
                        config.customSaveDirectory = path;
                    }
                }
                if (GUILayout.Button("Reset", GUILayout.Width(60)))
                {
                    config.customSaveDirectory = "";
                }
                EditorGUILayout.EndHorizontal();

                config.filePrefix = EditorGUILayout.TextField("File Prefix", config.filePrefix);
                config.fileExtension = EditorGUILayout.TextField("File Extension", config.fileExtension);

                EditorGUILayout.LabelField("Current Save Path:", config.GetSaveDirectory());

                EditorGUI.indentLevel--;
            }

            GUILayout.Space(5);

            // Performance
            performanceFoldout = EditorGUILayout.Foldout(performanceFoldout, "Performance", true);
            if (performanceFoldout)
            {
                EditorGUI.indentLevel++;

                config.maxCacheSizeMB = EditorGUILayout.IntSlider(
                    "Max Cache Size (MB)",
                    config.maxCacheSizeMB,
                    1, 500
                );

                config.enableAutoSave = EditorGUILayout.Toggle("Enable Auto Save", config.enableAutoSave);

                if (config.enableAutoSave)
                {
                    config.autoSaveInterval = EditorGUILayout.Slider(
                        "Auto Save Interval (seconds)",
                        config.autoSaveInterval,
                        10f, 3600f
                    );
                }

                EditorGUI.indentLevel--;
            }

            GUILayout.Space(5);

            // Debug
            debugFoldout = EditorGUILayout.Foldout(debugFoldout, "Debug & Backup", true);
            if (debugFoldout)
            {
                EditorGUI.indentLevel++;

                config.enableVerboseLogging = EditorGUILayout.Toggle("Enable Verbose Logging", config.enableVerboseLogging);
                config.enableAutoBackup = EditorGUILayout.Toggle("Enable Auto Backup", config.enableAutoBackup);

                if (config.enableAutoBackup)
                {
                    config.maxBackupCount = EditorGUILayout.IntSlider(
                        "Max Backup Count",
                        config.maxBackupCount,
                        1, 10
                    );
                }

                EditorGUI.indentLevel--;
            }

            GUILayout.Space(20);

            // Validation
            if (!config.ValidateConfig())
            {
                EditorGUILayout.HelpBox("Configuration has validation errors. Check console for details.", MessageType.Error);
            }

            // Buttons
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Save Configuration"))
            {
                SaveDataProConfig.SaveConfig();
                EditorUtility.DisplayDialog("Success", "Configuration saved successfully!", "OK");
            }

            if (GUILayout.Button("Reset to Default"))
            {
                if (EditorUtility.DisplayDialog("Confirm", "Reset all settings to default values?", "Yes", "Cancel"))
                {
                    SaveDataProConfig.ResetToDefault();
                    LoadConfig();
                    EditorUtility.DisplayDialog("Success", "Configuration reset to default!", "OK");
                }
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            // Info panel
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Information", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Config File: {Path.Combine(Application.persistentDataPath, "SaveDataProConfig.json")}");
            EditorGUILayout.LabelField($"Save Directory: {config.GetSaveDirectory()}");
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndScrollView();
        }
    }

}