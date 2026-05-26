using UnityEngine;
using UnityEngine.Localization;

public enum UpgradeEffect
{
    UnlockDash,
    UnlockHeal,
    UnlockLight,
    UnlockInvincibility,
    UnlockMakeMagicLight
}

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/Upgrade")]
public class UpgradeData : ScriptableObject
{
    [Header("Meta")]
    public string upgradeId;
    public bool singleUse = true;

    [Header("Localization Keys")]
    public LocalizedString titleKey;
    public LocalizedString descriptionKey;

    public LocalizedString parameter1Key;
    public LocalizedString parameter2Key;

    public LocalizedString characteristic1Key;
    public LocalizedString characteristic2Key;

    [Header("Effect")]
    public UpgradeEffect effect;
}
