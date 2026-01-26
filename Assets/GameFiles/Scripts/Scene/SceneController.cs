using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    [SerializeField] private Animator animatorBlackScreen;

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
        }    
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadSceneRoutine(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void ReloadScene(int buildIndex)
    {
        StartCoroutine(LoadSceneRoutine(buildIndex));
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadSceneRoutine(0));
    }

    private IEnumerator LoadSceneRoutine(int index)
    {
        animatorBlackScreen.SetTrigger("End");
        yield return new WaitForSeconds(2.0f);

        yield return SceneManager.LoadSceneAsync(index);       

        yield return null;

        animatorBlackScreen.SetTrigger("Start");
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}
