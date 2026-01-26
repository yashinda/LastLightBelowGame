using TMPro;
using UnityEngine;

public class StatisticsLevel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LevelStateController gameManager;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text secretsText;
    [SerializeField] private TMP_Text svetlesText;
    [SerializeField] private GameObject panelStatictics;

    [Header("CountStatisticOnLevel")]
    [SerializeField] private int secretsCountOnLevel;
    [SerializeField] private int svetlesCountOnLevel;

    private float currentTime;
    private int secretsFound;
    private int svetlesFound;
    private bool timerRunning;

    void Start()
    {
        timerRunning = true;
        currentTime = 0f;
        secretsFound = 0;
        svetlesFound = 0;
    }

    public void AddSecret()
    {
        secretsFound++;
    }

    public void AddSvetlesToStatictics(int svetles)
    {
        svetlesFound += svetles;
    }

    private void Update()
    {
        if (timerRunning && gameManager.CurrentState == LevelState.Playing)
            currentTime += Time.deltaTime;
    }

    public void ShowPanel()
    {
        timerRunning = false;
        panelStatictics.SetActive(true);

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        secretsText.text = $"{secretsFound} / {secretsCountOnLevel}";
        svetlesText.text = $"{svetlesFound} / {svetlesCountOnLevel}";
    }
}
