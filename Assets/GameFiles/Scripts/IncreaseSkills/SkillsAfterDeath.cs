using UnityEngine;

public static class SkillsAfterDeath
{
    public const string ReincartationKey = "Reincarnation";
    public const string BoostMoveSpeedKey = "BoostMoveSpeed";
    
    public static int ReincarnationAmount => PlayerPrefs.GetInt(ReincartationKey, 0);
    public static float BoostMoveSpeed => PlayerPrefs.GetFloat(BoostMoveSpeedKey, 1.0f);

    public static void AddBoostMoveSpeed()
    {
        PlayerPrefs.SetFloat(BoostMoveSpeedKey, BoostMoveSpeed + 0.05f);
        PlayerPrefs.Save();
    }

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
