using UnityEngine;

public class IncreaseSkills : MonoBehaviour
{
    public static IncreaseSkills Instance;

    [SerializeField] private SvetlesContainer svetlesContainer;

    private int dashLevel;
    private int lightLevel;
    private int healLevel;
    private int invincibilityLevel;

    private readonly float[] dashDistances = { 10.0f, 12.0f, 13.0f, 13.4f };
    private readonly float[] dashCooldowns = { 7.0f, 11.0f, 10.0f, 9.5f, 9.0f };
    private readonly int[] dashCosts = { 100, 200, 350, 600 };

    private readonly float[] lightDurations = { 3.0f, 4.0f, 5.0f, 5.5f, 6.0f };
    private readonly float[] lightCooldowns = { 15.0f, 23.0f, 22.0f, 21.0f, 20.0f };
    private readonly int[] lightCosts = { 200, 380, 600, 900 };

    private readonly float[] healAmounts = { 25.0f, 30.0f, 35.0f, 40.0f, 50.0f };
    private readonly float[] healCooldowns = { 15.0f, 28.0f, 26.0f, 26.5f, 25.0f };
    private readonly int[] healCosts = { 250, 400, 600, 900 };

    private readonly float[] invincibilityDurations = { 3.0f, 4.0f, 4.5f, 5.0f, 5.3f };
    private readonly float[] invincibilityCooldowns = { 18.0f, 31.0f, 30.0f, 29.5f, 29.0f };
    private readonly int[] invincibilityCosts = { 300, 450, 700, 1000 };

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        dashLevel = PlayerPrefs.GetInt("DashLevel", 0);
        lightLevel = PlayerPrefs.GetInt("LightLevel", 0);
        healLevel = PlayerPrefs.GetInt("HealLevel", 0);
        invincibilityLevel = PlayerPrefs.GetInt("InvincibilityLevel", 0);
    }

    public bool UpgradeDash()
    {
        return TryUpgrade(ref dashLevel, dashCosts, "DashLevel");
    }

    public bool UpgradeLight()
    {
        return TryUpgrade(ref lightLevel, lightCosts, "LightLevel");
    }

    public bool UpgradeHeal()
    {
        return TryUpgrade(ref healLevel, healCosts, "HealLevel");
    }

    public bool UpgradeInvincibility()
    {
        return TryUpgrade(ref invincibilityLevel, invincibilityCosts, "InvincibilityLevel");
    }

    private bool TryUpgrade(ref int level, int[] costs, string key)
    {
        if (level >= costs.Length - 1)
            return false;

        int cost = costs[level];

        if (svetlesContainer.CurrentSvetles < cost)
            return false;

        svetlesContainer.SpendSvetles(cost);
        level++;
        PlayerPrefs.SetInt(key, level);
        PlayerPrefs.Save();
        return true;
    }

    public float GetDashDistance() => dashDistances[Mathf.Clamp(dashLevel, 0, dashDistances.Length - 1)];
    public float GetDashCooldown() => dashCooldowns[Mathf.Clamp(dashLevel, 0, dashCooldowns.Length - 1)];

    public float GetLightDuration() => lightDurations[Mathf.Clamp(lightLevel, 0, lightDurations.Length - 1)];
    public float GetLightCooldown() => lightCooldowns[Mathf.Clamp(lightLevel, 0, lightCooldowns.Length - 1)];

    public float GetHealAmount() => healAmounts[Mathf.Clamp(healLevel, 0, healAmounts.Length - 1)];
    public float GetHealCooldown() => healCooldowns[Mathf.Clamp(healLevel, 0, healCooldowns.Length - 1)];

    public float GetInvincibilityDuration() => invincibilityDurations[Mathf.Clamp(invincibilityLevel, 0, invincibilityDurations.Length - 1)];
    public float GetInvincibilityCooldown() => invincibilityCooldowns[Mathf.Clamp(invincibilityLevel, 0, invincibilityCooldowns.Length - 1)];
}
