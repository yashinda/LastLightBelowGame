using UnityEngine;

public class GetUpgrade : MonoBehaviour
{
    [SerializeField] private GameObject panelUpgrade;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private UpgradeCardUI[] cards;
    [SerializeField] private AudioClip showClip;
    [SerializeField] private AudioSource audioSource;

    public void ShowPanel()
    {
        panelUpgrade.SetActive(true);
        audioSource.PlayOneShot(showClip);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        var upgrades = upgradeManager.GetRandomUpgrades(cards.Length);

        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].gameObject.SetActive(i < upgrades.Count);
            if (i < upgrades.Count)
                cards[i].Setup(upgrades[i], upgradeManager);
        }
    }

    public void DisablePanel()
    {
        panelUpgrade.SetActive(false);
    }
}
