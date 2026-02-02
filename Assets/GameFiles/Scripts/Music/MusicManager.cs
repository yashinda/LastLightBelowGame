using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public enum MusicState
{
    Exploration,
    Combat
}

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private MusicLibrary musicLibrary;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float minVolumeValue = 0.0f;
    [SerializeField] private float maxVolumeValue = 0.2f;

    private int activeEnemies;
    private MusicState currentState = MusicState.Exploration;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        PlayMusic(MusicState.Exploration.ToString());
    }

    public void PlayMusic(string trackName, float fadeDuration = 0.5f)
    {
        StartCoroutine(AnimateMusicCrossfade(musicLibrary.GetClipFromName(trackName)));
    }

    IEnumerator AnimateMusicCrossfade(AudioClip nextTrack)
    {
        float percent = 0.0f;
        while (percent < 1.0f)
        {
            percent += (1.0f / fadeDuration) * Time.deltaTime;
            musicSource.volume = Mathf.Lerp(maxVolumeValue, minVolumeValue, percent);
            yield return null;
        }

        musicSource.clip = nextTrack;
        musicSource.Play();

        percent = 0.0f;
        while (percent < 1.0f)
        {
            percent += (1.0f / fadeDuration) * Time.deltaTime;
            musicSource.volume = Mathf.Lerp(minVolumeValue, maxVolumeValue, percent);
            yield return null;
        }
    }

    public void RegisterEnemy()
    {
        activeEnemies++;

        if (activeEnemies == 1 && currentState != MusicState.Combat)
        {
            currentState = MusicState.Combat;
            PlayMusic(MusicState.Combat.ToString());
        }
    }

    public void UnregisterEnemy()
    {
        activeEnemies--;

        if (activeEnemies <= 0 && currentState != MusicState.Exploration)
        {
            activeEnemies = 0;
            currentState = MusicState.Exploration;
            PlayMusic(MusicState.Exploration.ToString());
        }
    }
}
