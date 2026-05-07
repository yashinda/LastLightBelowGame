using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Dropdown dropdownResolution;

    public Slider sliderMusic;
    public Slider sliderSounds;
    public Slider sliderSensitivity;

    public TMP_Text textValueMusic;
    public TMP_Text textValueSounds;
    public TMP_Text textSensitivity;

    [Header("Audio")]
    public AudioMixer mixerSettings;

    [Header("Audio Settings")]
    public float minMusicValue = -47.0f;
    public float maxMusicValue = 0.0f;

    public float minSoundValue = -47.0f;
    public float maxSoundValue = 0.0f;

    [Header("Sensitivity Settings")]
    public float minSensitivity = 1.0f;
    public float maxSensitivity = 6.0f;

    public float sensitivity;
    public float musicVolume;
    public float soundVolume;

    private List<Vector2Int> allowedResol = new()
    {
        new Vector2Int(1280, 720),
        new Vector2Int(1650, 1050),
        new Vector2Int(1920, 1080),
        new Vector2Int(2560, 1080),
        new Vector2Int(2560, 1440),
        new Vector2Int(3840, 2160),
        new Vector2Int(5120, 1440),
        new Vector2Int(5120, 2160)
    };

    private void Awake()
    {
        SetupResolution();
        SetupMusicSlider();
        SetupSoundSlider();
        SetupSensitivitySlider();
        LoadGraphicSettings();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void SetupResolution()
    {
        dropdownResolution.ClearOptions();

        List<string> options = allowedResol.Select(r => $"{r.x} x {r.y}").ToList();

        dropdownResolution.AddOptions(options);

        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", allowedResol.Count - 1);

        dropdownResolution.value = savedIndex;
        dropdownResolution.RefreshShownValue();

        ApplyResolution(savedIndex);

        dropdownResolution.onValueChanged.AddListener(OnResolutionChanged);
    }

    private void SetupMusicSlider()
    {
        sliderMusic.minValue = minMusicValue;
        sliderMusic.maxValue = maxMusicValue;

        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.0f);

        sliderMusic.value = savedMusicVolume;

        ChangeMusicValue(savedMusicVolume);

        sliderMusic.onValueChanged.AddListener(ChangeMusicValue);
    }

    private void SetupSoundSlider()
    {
        sliderSounds.minValue = minSoundValue;
        sliderSounds.maxValue = maxSoundValue;

        float savedSoundVolume = PlayerPrefs.GetFloat("SoundVolume", 0.0f);

        sliderSounds.value = savedSoundVolume;

        ChangeSoundValue(savedSoundVolume);

        sliderSounds.onValueChanged.AddListener(ChangeSoundValue);
    }

    private void SetupSensitivitySlider()
    {
        sliderSensitivity.minValue = minSensitivity;
        sliderSensitivity.maxValue = maxSensitivity;

        float savedSensitivity = PlayerPrefs.GetFloat("SensitivityValue", 1.0f);

        sliderSensitivity.value = savedSensitivity;

        ChangeSensitivity(savedSensitivity);

        sliderSensitivity.onValueChanged.AddListener(ChangeSensitivity);
    }

    private void LoadGraphicSettings()
    {
        int graphicLevel = PlayerPrefs.GetInt("GraphicLevel", QualitySettings.GetQualityLevel());

        QualitySettings.SetQualityLevel(graphicLevel);
    }

    private void ApplyResolution(int index)
    {
        Vector2Int selected = allowedResol[index];

        Screen.SetResolution(selected.x, selected.y, Screen.fullScreen);
    }

    private void UpdateTexts()
    {
        float musicPercent = (sliderMusic.value - minMusicValue) / -minMusicValue * 100f;
        float soundPercent = (sliderSounds.value - minSoundValue) / -minSoundValue * 100f;

        textValueMusic.text = Mathf.RoundToInt(musicPercent).ToString();

        textValueSounds.text = Mathf.RoundToInt(soundPercent).ToString();

        textSensitivity.text = Mathf.RoundToInt(sliderSensitivity.value).ToString();
    }

    public void ChangeMusicValue(float sliderValue)
    {
        musicVolume = sliderValue;

        mixerSettings.SetFloat("VolumeMusic", musicVolume);

        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();

        UpdateTexts();
    }

    public void ChangeSoundValue(float sliderValue)
    {
        soundVolume = sliderValue;

        mixerSettings.SetFloat("VolumeSound", soundVolume);

        PlayerPrefs.SetFloat("SoundVolume", soundVolume);
        PlayerPrefs.Save();

        UpdateTexts();
    }

    public void ChangeSensitivity(float value)
    {
        sensitivity = value;

        PlayerPrefs.SetFloat("SensitivityValue", value);
        PlayerPrefs.Save();

        UpdateTexts();
    }

    public void OnResolutionChanged(int index)
    {
        ApplyResolution(index);

        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();
    }

    public void SetGraphicLevel(int index)
    {
        QualitySettings.SetQualityLevel(index);
        QualitySettings.vSyncCount = 0;

        PlayerPrefs.SetInt("GraphicLevel", index);
        PlayerPrefs.Save();

        Debug.Log($"Set Graphic Level: {QualitySettings.names[QualitySettings.GetQualityLevel()]}");
    }
}