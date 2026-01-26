using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private UpgradeDatabase database;
    [SerializeField] private AbilityManager abilityManager;

    private HashSet<string> obtainedUpgrades = new();

    public List<UpgradeData> GetRandomUpgrades(int count)
    {
        var available = database.allUpgrades
            .Where(u => !u.singleUse || !obtainedUpgrades.Contains(u.upgradeId))
            .ToList();

        if (available.Count < count)
            count = available.Count;

        List<UpgradeData> result = new();

        for (int i = 0; i < count; i++)
        {
            var random = available[Random.Range(0, available.Count)];
            result.Add(random);
            available.Remove(random);
        }

        return result;
    }

    public void ApplyUpgrade(UpgradeData upgrade)
    {
        switch (upgrade.effect)
        {
            case UpgradeEffect.UnlockDash:
                abilityManager.UnlockDash();
                break;

            case UpgradeEffect.UnlockHeal:
                abilityManager.UnlockHeal();
                break;

            case UpgradeEffect.UnlockLight:
                abilityManager.UnlockLight();
                break;

            case UpgradeEffect.UnlockInvincibility:
                abilityManager.UnlockInvincibility();
                break;
        }

        obtainedUpgrades.Add(upgrade.upgradeId);
    }
}
