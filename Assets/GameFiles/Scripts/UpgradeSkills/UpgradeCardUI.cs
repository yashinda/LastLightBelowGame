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

        /*title.text = LocalizationManager.Instance.GetText(data.titleKey);

        description.text = LocalizationManager.Instance.GetText(data.descriptionKey);

        param1.text = LocalizationManager.Instance.GetText(data.parameter1Key);

        param2.text = LocalizationManager.Instance.GetText(data.parameter2Key);

        char1.text = LocalizationManager.Instance.GetText(data.characteristic1Key);

        char2.text = LocalizationManager.Instance.GetText(data.characteristic2Key);*/

        /*title.text = data.titleKey.ToString();
        description.text = data.descriptionKey.ToString();
        param1.text = data.parameter1Key.ToString();
        param2.text = data.parameter2Key.ToString();
        char1.text = data.characteristic1Key.ToString();
        char2.text = data.characteristic2Key.ToString();*/

        title.text = data.titleKey.GetLocalizedString();
        description.text = data.descriptionKey.GetLocalizedString();
        param1.text = data.parameter1Key.GetLocalizedString();
        param2.text = data.parameter2Key.GetLocalizedString();
        char1.text = data.characteristic1Key.GetLocalizedString();
        char2.text = data.characteristic2Key.GetLocalizedString();

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
