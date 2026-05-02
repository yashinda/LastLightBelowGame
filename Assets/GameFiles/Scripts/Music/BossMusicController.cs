using UnityEngine;
using System.Collections;

[System.Serializable]
public class BossMusicSet
{
    public AudioClip intro;
    public AudioClip phase1Loop;
    public AudioClip transition;
    public AudioClip phase2Loop;
    public AudioClip outro;
}

public class BossMusicController : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private BossMusicSet musicSet;

    private Coroutine currentRoutine;

    private void Start()
    {
        StartBossMusic();
    }

    public void StartBossMusic()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(PlayIntroAndLoop1());
    }

    private IEnumerator PlayIntroAndLoop1()
    {
        musicSource.loop = false;
        musicSource.clip = musicSet.intro;
        musicSource.Play();

        yield return new WaitForSeconds(musicSet.intro.length);

        musicSource.loop = true;
        musicSource.clip = musicSet.phase1Loop;
        musicSource.Play();
    }

    public void StartSecondPhase()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(TransitionToPhase2());
    }

    private IEnumerator TransitionToPhase2()
    {
        musicSource.loop = false;

        musicSource.clip = musicSet.transition;
        musicSource.Play();

        yield return new WaitForSeconds(musicSet.transition.length);

        musicSource.loop = true;
        musicSource.clip = musicSet.phase2Loop;
        musicSource.Play();
    }

    public void EndBossFight()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        StartCoroutine(PlayOutro());
    }

    private IEnumerator PlayOutro()
    {
        musicSource.loop = true;

        musicSource.clip = musicSet.outro;
        musicSource.Play();

        yield return null;
    }
}
