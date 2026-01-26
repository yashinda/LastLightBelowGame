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

        title.text = data.title;
        description.text = data.description;

        param1.text = data.textParameter1;
        param2.text = data.textParameter2;

        char1.text = data.characteristic1;
        char2.text = data.characteristic2;

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
