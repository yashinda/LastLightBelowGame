using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public TMP_Dropdown dropdownResolution;
    public Slider sliderMusic;
    public Slider sliderSounds;

    private List<Vector2Int> allowedResol = new List<Vector2Int>()
    {
        new Vector2Int(1280, 720),
        new Vector2Int(1650, 1050),
        new Vector2Int(1920, 1080),
        new Vector2Int(2560, 1440)
    };

    public float maxMusicValue = 1.0f;
    public float minMusicValue = 0.0f;
    public float maxSoundValue = 1.0f;
    public float minSoundValue = 0.0f;

    private void Start()
    {
        dropdownResolution.ClearOptions();
        List<string> optionsResolution = allowedResol.Select(res => $"{res.x} x {res.y}").ToList();
        dropdownResolution.AddOptions(optionsResolution);
        dropdownResolution.onValueChanged.AddListener(OnResolutionChanged);

        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", allowedResol.Count - 1);
        dropdownResolution.value = savedIndex;
        dropdownResolution.RefreshShownValue();
        OnResolutionChanged(savedIndex);
    }

    private void OnResolutionChanged(int index)
    {
        Vector2Int selectedRes = allowedResol[index];
        Screen.SetResolution(selectedRes.x, selectedRes.y, Screen.fullScreen);

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
