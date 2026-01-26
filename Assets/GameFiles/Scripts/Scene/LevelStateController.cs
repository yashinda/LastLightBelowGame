using UnityEngine;
using UnityEngine.SceneManagement;

public enum LevelState
{
    Playing,
    Paused,
    PlayerDead,
    ChooseUpgrade,
    LevelCompleted
}

public class LevelStateController : MonoBehaviour
{
    public static LevelStateController Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject deathPanel;

    [Header("References")]
    [SerializeField] private StatisticsLevel statisticsLevel;

    private LevelState currentState = LevelState.Playing;

    public LevelState CurrentState => currentState;
    public bool IsPaused => currentState == LevelState.Paused;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /*private void Start()
    {
        EnterPlayingState();
    }*/

    #region Public API

    public void TogglePause()
    {
        if (currentState == LevelState.PlayerDead || currentState == LevelState.LevelCompleted || currentState == LevelState.ChooseUpgrade)
            return;

        if (currentState == LevelState.Playing)
            EnterPauseState();
        else if (currentState == LevelState.Paused)
            EnterPlayingState();
    }

    public void PlayerDied()
    {
        if (currentState == LevelState.PlayerDead)
            return;

        if (deathPanel != null)
            deathPanel.SetActive(true);

        RestartLevel();

        currentState = LevelState.PlayerDead; 
    }

    public void CompleteLevel()
    {
        if (currentState == LevelState.LevelCompleted)
            return;

        currentState = LevelState.LevelCompleted;

        Time.timeScale = 0f;
        ShowCursor(true);

        statisticsLevel?.ShowPanel();
    }

    public void PlayerChoosesUpgrade()
    {
        currentState = LevelState.ChooseUpgrade;
        ShowCursor(true);
    }

    public void PlayerChoseUpgrade()
    {
        currentState = LevelState.Playing;
        ShowCursor(false);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneController.Instance.ReloadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneController.Instance.LoadMainMenu();
    }

    #endregion

    #region State Transitions

    private void EnterPlayingState()
    {
        currentState = LevelState.Playing;

        Time.timeScale = 1f;
        ShowCursor(false);

        pausePanel?.SetActive(false);
    }

    private void EnterPauseState()
    {
        currentState = LevelState.Paused;

        Time.timeScale = 0f;
        ShowCursor(true);

        pausePanel?.SetActive(true);
    }

    #endregion

    private void ShowCursor(bool show)
    {
        Cursor.visible = show;
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
    }
}