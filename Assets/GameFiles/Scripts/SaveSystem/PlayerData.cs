using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

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
    public int rifleDamage;
    public bool unlockDashAbil;
    public bool unlockHealAbil;
    public bool unlockLightAbil;
    public bool unlockInvincibilityAbil;
    public bool unlockMakeMagicLightAbil;
    public int indexScene = 0;

    public PlayerData(
        SvetlesContainer svetlesContainer,
        UpgradeManager upgradeManager,
        PlayerHealth playerHealth,
        PsySystem psySystem,
        Shotgun shotgun,
        Revolver revolver,
        Rifle rifle,
        AbilityManager abilityManager)
    {
        svetlesAmount = svetlesContainer.CurrentSvetles;
        obtainedUpgrades = upgradeManager.GetObtainedUpgrades().ToList();
        maxHealth = playerHealth.MaxHealth;
        maxPsyAmount = psySystem.maxPsyAmount;
        armor = playerHealth.Armor;
        shotgunDamage = shotgun.Damage;
        revolverDamage = revolver.Damage;
        rifleDamage = rifle.Damage;
        unlockDashAbil = abilityManager.unlockDash;
        unlockHealAbil = abilityManager.unlockHeal;
        unlockLightAbil = abilityManager.unlockLight;
        unlockInvincibilityAbil = abilityManager.unlockInvincible;
        unlockMakeMagicLightAbil = abilityManager.unlockMakeMagicLight;
        if (LevelStateController.Instance.CurrentState == LevelState.ChooseUpgrade)
            indexScene = SceneManager.GetActiveScene().buildIndex;
        else
            indexScene = SceneManager.GetActiveScene().buildIndex + 1;
    }
}
