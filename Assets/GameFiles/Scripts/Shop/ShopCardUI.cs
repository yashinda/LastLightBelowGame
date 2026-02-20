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

    [Header("References")]
    [SerializeField] private SvetlesContainer svetlesContainer;
    [SerializeField] private PurchaseType purchaseType;
    [SerializeField] private Shop shop;

    [Header("ButtonSound")]
    [SerializeField] private AudioClip HPandPsiClip;
    [SerializeField] private AudioClip armorClip;
    [SerializeField] private AudioClip weaponClip;
    [SerializeField] private AudioSource audioSource;
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
            return;

        switch(purchaseType)
        {
            case PurchaseType.IncreaseMaxHP:
                shop.PurchaseMaxHP(price);
                audioSource.PlayOneShot(HPandPsiClip);
                break;
            case PurchaseType.IncreaseMaxPsi:
                shop.PurchaseMaxPsi(price);
                audioSource.PlayOneShot(HPandPsiClip);
                break;
            case PurchaseType.GetArmor:
                shop.PurchaseArmor(price);
                audioSource.PlayOneShot(armorClip);
                break;
            case PurchaseType.IncreaseShotgun:
                shop.IncreaseShotgun(price);
                audioSource.PlayOneShot(weaponClip);
                break;
            case PurchaseType.IncreaseRevolver:
                shop.IncreaseRevolver(price);
                audioSource.PlayOneShot(weaponClip);
                break;
        }

        button.gameObject.SetActive(false);
        textSvetles.SetActive(false);
        textCountSvetles.SetActive(false);
        textPurchased.SetActive(true);
    }
}
