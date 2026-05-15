using UnityEngine;

public static class SkillsAfterDeath
{
    public const string ReincartationKey = "Reincarnation";
    
    public static int Amount => PlayerPrefs.GetInt(ReincartationKey, 0);

    public static void Add(int amount)
    {
        PlayerPrefs.SetInt(ReincartationKey, Amount + amount);
        PlayerPrefs.Save();
    }

    public static void RemoveReincarnation()
    {
        PlayerPrefs.SetInt(ReincartationKey, Amount - 1);
        PlayerPrefs.Save();
    }
}
