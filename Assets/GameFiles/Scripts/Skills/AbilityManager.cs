using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [Header("IconsAbilities")]
    [SerializeField] private AbilityIconUI dashIcon;
    [SerializeField] private GameObject lockDashImage;
    [SerializeField] private AbilityIconUI healIcon;
    [SerializeField] private GameObject lockHealImage;
    [SerializeField] private AbilityIconUI invincibilityIcon;
    [SerializeField] private GameObject lockInvincibilityImage;
    [SerializeField] private AbilityIconUI lightIcon;
    [SerializeField] private GameObject lockLightImage;

    private Dash dash;
    private PlayerHealingAbility heal;
    private CreateLight lightAbility;
    private PlayerInvincibilityAbility invincibility;

    public bool unlockDash = false;
    public bool unlockHeal = false;
    public bool unlockLight = false;
    public bool unlockInvincible = false;

    private void Start()
    {
        if (unlockDash)
            UnlockDash();

        if (unlockHeal)
            UnlockHeal();

        if (unlockLight)
            UnlockLight();

        if (unlockInvincible)
            UnlockInvincibility();
    }

    public void UnlockDash()
    {
        if (dash != null) return;
        dash = player.AddComponent<Dash>();

        lockDashImage.SetActive(false);
        dashIcon.Bind(dash);
        unlockDash = true;
    }

    public void UnlockHeal()
    {
        if (heal != null) return;
        heal = player.AddComponent<PlayerHealingAbility>();
        
        lockHealImage.SetActive(false);
        healIcon.Bind(heal);
        unlockHeal = true;
    }

    public void UnlockLight()
    {
        if (lightAbility != null) return;
        lightAbility = player.AddComponent<CreateLight>();

        lockLightImage.SetActive(false);
        lightIcon.Bind(lightAbility);
        unlockLight = true;
    }

    public void UnlockInvincibility()
    {
        if (invincibility != null) return;
        invincibility = player.AddComponent<PlayerInvincibilityAbility>();
        
        lockInvincibilityImage.SetActive(false);
        invincibilityIcon.Bind(invincibility);
        unlockInvincible = true;
    }
}
