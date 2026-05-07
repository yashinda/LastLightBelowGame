using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject buttonContinue;

    private void Start()
    {
        buttonContinue.SetActive(SceneController.GetIndexScene() != 0);
    }

    public void OnNewGame()
    {
        SceneController.Instance.LoadNewGame();
    }

    public void OnContinueGame()
    {
        SceneController.Instance.ContinueGame();
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
