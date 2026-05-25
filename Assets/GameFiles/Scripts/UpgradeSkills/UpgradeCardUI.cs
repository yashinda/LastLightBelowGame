using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCardUI : MonoBehaviour
{
    [SerializeField] private LevelStateController gameManager;
    [SerializeField] private GetUpgrade upgradeSystem;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text param1;
    [SerializeField] private TMP_Text param2;
    [SerializeField] private TMP_Text char1;
    [SerializeField] private TMP_Text char2;

    private UpgradeData currentUpgrade;
    private UpgradeManager manager;
    private Button button;

    public void Setup(UpgradeData data, UpgradeManager upgradeManager)
    {
        currentUpgrade = data;
        manager = upgradeManager;

        title.text = LocalizationManager.Instance.GetText(data.titleKey);

        description.text = LocalizationManager.Instance.GetText(data.descriptionKey);

        param1.text = LocalizationManager.Instance.GetText(data.parameter1Key);

        param2.text = LocalizationManager.Instance.GetText(data.parameter2Key);

        char1.text = LocalizationManager.Instance.GetText(data.characteristic1Key);

        char2.text = LocalizationManager.Instance.GetText(data.characteristic2Key);

        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        manager.ApplyUpgrade(currentUpgrade);
        upgradeSystem.DisablePanel();
        gameManager.PlayerChoseUpgrade();
    }
}
