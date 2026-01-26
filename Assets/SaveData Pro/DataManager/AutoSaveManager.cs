using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SaveDataPro.DataManager
{
    /// <summary>
    /// Manages auto save for registered objects
    /// </summary>
    public class AutoSaveManager : MonoBehaviour
    {
        private class AutoSaveEntry
        {
            public object Data;
            public string Key;
        }

        private static AutoSaveManager _instance;
        public static AutoSaveManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("AutoSaveManager");
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<AutoSaveManager>();
                }
                return _instance;
            }
        }

        private readonly List<AutoSaveEntry> autoSaveEntries = new List<AutoSaveEntry>();
        private Coroutine autoSaveCoroutine;

        /// <summary>
        /// Register object for auto save with key
        /// </summary>
        public void Register(object data, string key)
        {
            if (string.IsNullOrEmpty(key) || data == null)
                return;
            // Check for duplicate key
            foreach (var entry in autoSaveEntries)
            {
                if (entry.Key == key)
                {
                    entry.Data = data;
                    return;
                }
            }
            autoSaveEntries.Add(new AutoSaveEntry { Data = data, Key = key });
            StartAutoSaveIfNeeded();
        }

        /// <summary>
        /// Unregister auto save for key
        /// </summary>
        public void Unregister(string key)
        {
            autoSaveEntries.RemoveAll(e => e.Key == key);
        }

        private void StartAutoSaveIfNeeded()
        {
            if (autoSaveCoroutine == null && SaveDataProConfig.Instance.enableAutoSave)
            {
                autoSaveCoroutine = StartCoroutine(AutoSaveRoutine());
            }
        }

        private IEnumerator AutoSaveRoutine()
        {
            while (true)
            {
                float interval = Mathf.Max(1f, SaveDataProConfig.Instance.autoSaveInterval);
                yield return new WaitForSeconds(interval);
                foreach (var entry in autoSaveEntries)
                {
                    if (entry.Data != null && !string.IsNullOrEmpty(entry.Key))
                    {
                        DataManager.Save(entry.Key, entry.Data);
                        Debug.Log($"[AutoSave] Saved key: {entry.Key}");
                    }
                }
            }
        }
    }
}
