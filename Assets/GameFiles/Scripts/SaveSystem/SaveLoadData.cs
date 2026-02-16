using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoadData
{
    private static readonly string savePath = Application.persistentDataPath + "/save.b";

    public static void SaveGame(SvetlesContainer svetlesContainer, UpgradeManager upgradeManager, PlayerHealth playerHealth, PsySystem psySystem,
        Shotgun shotgun, Revolver revolver, AbilityManager abilityManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Create);

        PlayerData data = new PlayerData(svetlesContainer, upgradeManager, playerHealth, psySystem, shotgun, revolver, abilityManager);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void LoadGame(SvetlesContainer svetlesContainer, UpgradeManager upgradeManager, PlayerHealth playerHealth, PsySystem psySystem,
        Shotgun shotgun, Revolver revolver, AbilityManager abilityManager)
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            svetlesContainer.SetSvetlesAmount(data.svetlesAmount);

            upgradeManager.LoadObtainedUpgrades(data.obtainedUpgrades);

            playerHealth.SetMaxHealth(data.maxHealth);
            psySystem.SetMaxPsyAmount(data.maxPsyAmount);
            playerHealth.SetArmor(data.armor);
            shotgun.SetDamage(data.shotgunDamage);
            revolver.SetDamage(data.revolverDamage);

            if (data.unlockDashAbil)
                abilityManager.SetUnlockDash();

            if (data.unlockHealAbil)
                abilityManager.SetUnlockHeal();

            if (data.unlockLightAbil)
                abilityManager.SetUnlockLight();

            if (data.unlockInvincibilityAbil)
                abilityManager.SetUnlockInvincibility();

            abilityManager.unlockDash = data.unlockDashAbil;
            abilityManager.unlockHeal = data.unlockHealAbil;
            abilityManager.unlockLight = data.unlockLightAbil;
            abilityManager.unlockInvincible = data.unlockInvincibilityAbil;
        }
    }
}
