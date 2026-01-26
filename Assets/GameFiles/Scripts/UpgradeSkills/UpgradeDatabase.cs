using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeDatabase", menuName = "Upgrades/UpgradeDatabase")]
public class UpgradeDatabase : ScriptableObject
{
    public List<UpgradeData> allUpgrades;
}
