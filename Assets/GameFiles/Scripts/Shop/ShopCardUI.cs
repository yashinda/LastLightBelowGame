using UnityEngine;
using UnityEngine.UI;

public enum PurchaseType
{
    IncreaseMaxHP,
    IncreaseMaxPsi,
    GetArmor,
    IncreaseShotgun,
    IncreaseRevolver
}

public class ShopCardUI : MonoBehaviour
{
    [SerializeField] private GameObject textSvetles;
    [SerializeField] private GameObject textCountSvetles;
    [SerializeField] private GameObject textPurchased;
    [SerializeField] private int price;
    [SerializeField] private SvetlesContainer svetlesContainer;
    [SerializeField] private PurchaseType purchaseType;
    [SerializeField] private Shop shop;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Purchase);
    }

    private bool CanPurchased() => price < svetlesContainer.CurrentSvetles;
    public void Purchase()
    {
        if (!CanPurchased())
        {
            Debug.Log("Не хватает денег");
            return;
        }

        switch(purchaseType)
        {
            case PurchaseType.IncreaseMaxHP:
                shop.PurchaseMaxHP(price);
                break;
            case PurchaseType.IncreaseMaxPsi:
                shop.PurchaseMaxPsi(price);
                break;
            case PurchaseType.GetArmor:
                shop.PurchaseArmor(price);
                break;
            case PurchaseType.IncreaseShotgun:
                shop.IncreaseShotgun(price);
                break;
            case PurchaseType.IncreaseRevolver:
                shop.IncreaseRevolver(price);
                break;
        }

        button.gameObject.SetActive(false);
        textSvetles.SetActive(false);
        textCountSvetles.SetActive(false);
        textPurchased.SetActive(true);
    }
}
