using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    public void OnNewGame()
    {
        SceneController.Instance.LoadNextLevel();
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
