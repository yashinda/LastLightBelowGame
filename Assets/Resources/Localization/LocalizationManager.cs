using System.Collections.Generic;
using UnityEngine;

public enum Language
{
    English,
    Russian
}

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    [SerializeField] private Language currentLanguage;

    private Dictionary<string, string> localizedText = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadLocalization();
    }

    public void SetLanguage(Language language)
    {
        currentLanguage = language;
        LoadLocalization();
    }

    private void LoadLocalization()
    {
        localizedText.Clear();

        TextAsset jsonFile = Resources.Load<TextAsset>("Localization/localization");

        if (jsonFile == null)
        {
            Debug.LogError("Localization JSON not found!");
            return;
        }

        LocalizationRoot data = JsonUtility.FromJson<LocalizationRoot>(jsonFile.text);

        LocalizationLanguage selectedLanguage = currentLanguage switch { Language.English => data.English, Language.Russian => data.Russian, _ => data.English };

        foreach (var entry in selectedLanguage.entries)
        {
            localizedText[entry.key] = entry.value;
        }
    }

    public string GetText(string key)
    {
        if (localizedText.TryGetValue(key, out string value))
            return value;

        Debug.LogWarning($"Localization key not found: {key}");

        return key;
    }
}