using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    public void OnResume()
    {
        LevelStateController.Instance.TogglePause();
    }

    public void OnRestart()
    {
        LevelStateController.Instance.RestartLevel();
    }

    public void OnExitToMenu()
    {
        LevelStateController.Instance.ExitToMainMenu();
    }

    public void OnExitToDesktop()
    {
        Application.Quit();
    }

}
