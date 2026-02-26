using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    public void OnNewGame()
    {
        SceneController.Instance.LoadNextLevel();
        SaveLoadData.LoadNewGame();
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
