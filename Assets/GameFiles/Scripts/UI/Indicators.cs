using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Indicators : MonoBehaviour
{
    public TMP_Text svetlesCount;

    public PlayerHealth playerHealth;
    public PsySystem psySystem;
    public SvetlesContainer svetlesContainer;

    public TMP_Text healthAmountText;
    public TMP_Text psyAmountText;
    public TMP_Text armorAmountText;

    public Image healthBar;
    public Image psiBar;

    public GameObject imageMadness;

    public GameObject panelEchoSvetles;
    public TMP_Text echoSvetlesText;
    
    private void Update()
    {
        if (!playerHealth.PlayerDead)
        {
            svetlesCount.text = svetlesContainer.CurrentSvetles.ToString();
            healthAmountText.text = $"{Mathf.FloorToInt(playerHealth.CurrentHealth)}/{Mathf.FloorToInt(playerHealth.MaxHealth)}".ToString();
            psyAmountText.text = $"{Mathf.FloorToInt(psySystem.psyAmount)}/{Mathf.FloorToInt(psySystem.maxPsyAmount)}".ToString();
            armorAmountText.text = playerHealth.Armor.ToString();
            
            var healthBarAmount = playerHealth.CurrentHealth / playerHealth.MaxHealth;
            var psiBarAmount = psySystem.psyAmount / psySystem.maxPsyAmount;
            
            healthBar.fillAmount = healthBarAmount;
            psiBar.fillAmount = psiBarAmount;
            
            if (playerHealth.CurrentHealth > 30)
                healthAmountText.color = Color.white;
            else
                healthAmountText.color = Color.red;

            if (psySystem.madnessActive)
            {
                imageMadness.SetActive(true);
                psyAmountText.color = Color.orangeRed;
            }
            else
            {
                imageMadness.SetActive(false);
                psyAmountText.color = Color.white;
            }

            if (panelEchoSvetles != null && EchoSvetles.Amount > 0)
            {
                panelEchoSvetles.SetActive(true);
                echoSvetlesText.text = EchoSvetles.Amount.ToString();
            }
        }
    }
}
