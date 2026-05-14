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

    public GameObject imageMadness;

    public GameObject panelEchoSvetles;
    public TMP_Text echoSvetlesText;
    
    private void Update()
    {
        if (!playerHealth.PlayerDead)
        {
            svetlesCount.text = svetlesContainer.CurrentSvetles.ToString();
            healthAmountText.text = Mathf.FloorToInt(playerHealth.CurrentHealth).ToString();
            psyAmountText.text = Mathf.FloorToInt(psySystem.psyAmount).ToString();
            armorAmountText.text = playerHealth.Armor.ToString();

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
