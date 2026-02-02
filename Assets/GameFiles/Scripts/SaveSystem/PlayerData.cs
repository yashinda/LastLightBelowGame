using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int svetlesAmount;
    public List<string> obtainedUpgrades;
    public float maxHealth;
    public float maxPsyAmount;
    public int armor;
    public int shotgunDamage;
    public int revolverDamage;
    public bool unlockDashAbil;
    public bool unlockHealAbil;
    public bool unlockLightAbil;
    public bool unlockInvincibilityAbil;

    public PlayerData(
        SvetlesContainer svetlesContainer,
        UpgradeManager upgradeManager,
        PlayerHealth playerHealth,
        PsySystem psySystem,
        Shotgun shotgun,
        Revolver revolver,
        AbilityManager abilityManager)
    {
        svetlesAmount = svetlesContainer.CurrentSvetles;
        obtainedUpgrades = upgradeManager.GetObtainedUpgrades().ToList();
        maxHealth = playerHealth.MaxHealth;
        maxPsyAmount = psySystem.maxPsyAmount;
        armor = playerHealth.Armor;
        shotgunDamage = shotgun.Damage;
        revolverDamage = revolver.Damage;
        unlockDashAbil = abilityManager.unlockDash;
        unlockHealAbil = abilityManager.unlockHeal;
        unlockLightAbil = abilityManager.unlockLight;
        unlockInvincibilityAbil = abilityManager.unlockInvincible;
    }
}
