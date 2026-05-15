using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject buttonContinue;

    private void Start()
    {
        buttonContinue.SetActive(SceneController.GetSave());
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
