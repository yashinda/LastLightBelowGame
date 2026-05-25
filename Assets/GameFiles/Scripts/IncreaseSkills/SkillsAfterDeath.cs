using UnityEngine;

public static class SkillsAfterDeath
{
    public const string ReincartationKey = "Reincarnation";
    public const string BoostMoveSpeedKey = "BoostMoveSpeed";
    public const string BoostMaxHPKey = "BoostMaxHP";
    public const string BoostArmorKey = "BoostArmor";
    
    public static int ReincarnationAmount => PlayerPrefs.GetInt(ReincartationKey, 0);
    public static float BoostMoveSpeed => PlayerPrefs.GetFloat(BoostMoveSpeedKey, 1.0f);
    public static float BoostMaxHp => PlayerPrefs.GetFloat(BoostMaxHPKey, 0.0f);
    public static int BoostArmor => PlayerPrefs.GetInt(BoostArmorKey, 0);

    public static void AddBoostArmor()
    {
        PlayerPrefs.SetInt(BoostArmorKey, BoostArmor + 5);
        PlayerPrefs.Save();
    }
    
    public static void AddBoostMoveSpeed()
    {
        PlayerPrefs.SetFloat(BoostMoveSpeedKey, BoostMoveSpeed + 0.05f);
        PlayerPrefs.Save();
    }
    
    public static void AddBoostMaxHP()
    {
        PlayerPrefs.SetFloat(BoostMaxHPKey, BoostMaxHp + 5.0f);
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
