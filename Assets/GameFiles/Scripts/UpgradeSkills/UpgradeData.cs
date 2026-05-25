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
    public string titleKey;
    public string descriptionKey;

    public string parameter1Key;
    public string parameter2Key;

    public string characteristic1Key;
    public string characteristic2Key;

    [Header("Effect")]
    public UpgradeEffect effect;
}
