using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum UIButtonSoundType
{
    System,
    Ability,
    Confirm
}

public class UIButtonSound : MonoBehaviour,
    IPointerEnterHandler,
    ISelectHandler
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource uiAudioSource;

    [Header("Sound Type")]
    [SerializeField] private UIButtonSoundType soundType;

    [Header("Hover Clips")]
    [SerializeField] private AudioClip systemHover;
    [SerializeField] private AudioClip abilityHover;
    [SerializeField] private AudioClip confirmHover;

    [Header("Click Clips")]
    [SerializeField] private AudioClip systemClick;
    [SerializeField] private AudioClip abilityClick;
    [SerializeField] private AudioClip confirmClick;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayClickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverSound();
    }

    public void OnSelect(BaseEventData eventData)
    {
        PlayHoverSound();
    }

    private void PlayHoverSound()
    {
        AudioClip clip = soundType switch
        {
            UIButtonSoundType.System => systemHover,
            UIButtonSoundType.Ability => abilityHover,
            UIButtonSoundType.Confirm => confirmHover,
            _ => null
        };

        if (clip != null && uiAudioSource != null)
            uiAudioSource.PlayOneShot(clip);
    }

    private void PlayClickSound()
    {
        AudioClip clip = soundType switch
        {
            UIButtonSoundType.System => systemClick,
            UIButtonSoundType.Ability => abilityClick,
            UIButtonSoundType.Confirm => confirmClick,
            _ => null
        };

        if (clip != null && uiAudioSource != null)
            uiAudioSource.PlayOneShot(clip);
    }
}
