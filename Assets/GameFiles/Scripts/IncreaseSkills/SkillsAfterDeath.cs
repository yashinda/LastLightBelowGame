using UnityEngine;

public static class SkillsAfterDeath
{
    public const string ReincartationKey = "Reincarnation";
    
    public static int ReincarnationAmount => PlayerPrefs.GetInt(ReincartationKey, 0);

    public static void AddReincarnation()
    {
        PlayerPrefs.SetInt(ReincartationKey, ReincarnationAmount + 1);
        PlayerPrefs.Save();
    }

    public static void RemoveReincarnation()
    {
        PlayerPrefs.SetInt(ReincartationKey, ReincarnationAmount - 1);
        PlayerPrefs.Save();
    }
}
