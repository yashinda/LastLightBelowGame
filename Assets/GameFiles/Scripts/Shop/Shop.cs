using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private SvetlesContainer svetlesContainer;
    [SerializeField] private TMP_Text textSvetlesAmount;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PsySystem psySystem;
    [SerializeField] private Shotgun shotgun;
    [SerializeField] private Revolver revolver;
    [SerializeField] private float increaseMaxHPamount;
    [SerializeField] private float increaseMaxPsyAmount;
    [SerializeField] private int armorAmount;
    [SerializeField] private int increaseShotgunDamage;
    [SerializeField] private int inreaseRevolverDamage;
    [SerializeField] private GameObject armorIndicator;

    private void Start()
    {
        textSvetlesAmount.text = svetlesContainer.CurrentSvetles.ToString();
    }

    public void PurchaseMaxHP(int price)
    {        
        svetlesContainer.SpendSvetles(price);
        playerHealth.IncreaseMaxHP(increaseMaxHPamount);
        textSvetlesAmount.text = svetlesContainer.CurrentSvetles.ToString();
    }

    public void PurchaseMaxPsi(int price)
    {
        svetlesContainer.SpendSvetles(price);
        psySystem.IncreaseMaxPsi(increaseMaxPsyAmount);
        textSvetlesAmount.text = svetlesContainer.CurrentSvetles.ToString();
    }

    public void PurchaseArmor(int price)
    {
        svetlesContainer.SpendSvetles(price);
        armorIndicator.SetActive(true);
        playerHealth.GetArmor(armorAmount);
        textSvetlesAmount.text = svetlesContainer.CurrentSvetles.ToString();
    }

    public void IncreaseShotgun(int price)
    {
        svetlesContainer.SpendSvetles(price);
        shotgun.IncreaseDamage(increaseShotgunDamage);
        textSvetlesAmount.text = svetlesContainer.CurrentSvetles.ToString();
    }

    public void IncreaseRevolver(int price)
    {
        svetlesContainer.SpendSvetles(price);
        revolver.IncreaseDamage(inreaseRevolverDamage);
        textSvetlesAmount.text = svetlesContainer.CurrentSvetles.ToString();
    }
}
