using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

    public void LoadNewGame()
    {
        StartCoroutine(LoadSceneRoutine(1));
        SaveLoadData.LoadNewGame();
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneRoutine(GetIndexScene()));
        Debug.Log(GetIndexScene());
    }

    private IEnumerator LoadSceneRoutine(int index)
    {
        animatorBlackScreen.SetTrigger("End");
        yield return new WaitForSeconds(2.0f);

        yield return SceneManager.LoadSceneAsync(index);       

        yield return null;

        animatorBlackScreen.SetTrigger("Start");
    }

    public static int GetIndexScene()
    {
        string savePath = Application.persistentDataPath + "/save.b";

        if (!File.Exists(savePath))
            return 0;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Open);

        PlayerData data = formatter.Deserialize(stream) as PlayerData;

        stream.Close();

        return data.indexScene;
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}
