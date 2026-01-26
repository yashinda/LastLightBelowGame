using UnityEngine;

public enum UpgradeEffect
{
    UnlockDash,
    UnlockHeal,
    UnlockLight,
    UnlockInvincibility
}

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/Upgrade")]
public class UpgradeData : ScriptableObject
{
    [Header("Meta")]
    public string upgradeId;
    public bool singleUse = true;

    [Header("UI")]
    public string title;
    [TextArea] public string description;

    public string textParameter1;
    public string textParameter2;

    public string characteristic1;
    public string characteristic2;

    [Header("Effect")]
    public UpgradeEffect effect;
}
