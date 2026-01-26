using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Indicators : MonoBehaviour
{
    public Image indicatorHealth;
    public Image indicatorPsy;
    public TMP_Text svetlesCount;

    public PlayerHealth playerHealth;
    public PsySystem psySystem;
    public SvetlesContainer svetlesContainer;

    public TMP_Text healthAmountText;
    public TMP_Text psyAmountText;

    private void Update()
    {
        if (!playerHealth.PlayerDead)
        {
            indicatorHealth.fillAmount = HealthAmount;
            indicatorPsy.fillAmount = PsyAmount;
            svetlesCount.text = svetlesContainer.CurrentSvetles.ToString();
            healthAmountText.text = $"{Mathf.FloorToInt(playerHealth.CurrentHealth)}/{playerHealth.MaxHealth}";
            psyAmountText.text = $"{Mathf.FloorToInt(psySystem.psyAmount)}/{psySystem.maxPsyAmount}";
        }
        else
            indicatorHealth.fillAmount = 0;
    }

    public float HealthAmount => playerHealth.CurrentHealth / playerHealth.MaxHealth;
    public float PsyAmount => psySystem.psyAmount / psySystem.maxPsyAmount;
}
