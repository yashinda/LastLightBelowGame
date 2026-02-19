using UnityEngine;
using System.Collections;

public class CreateLight : ActiveAbility
{
    [SerializeField] private Light pointLight;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject effect;

    private float duration;

    protected override float CooldownDuration =>
        IncreaseSkills.Instance.GetLightCooldown();

    private void Start()
    {
        duration = IncreaseSkills.Instance.GetLightDuration();
        playerHealth = GetComponent<PlayerHealth>();
        pointLight = GetComponentInChildren<Light>();
        effect = GetComponentInChildren<Lightning>(true).gameObject;
    }

    protected override bool CanActivate()
    {
        return !playerHealth.PlayerDead;
    }

    protected override IEnumerator ActivateRoutine()
    {
        pointLight.enabled = true;
        yield return new WaitForSeconds(duration);
        pointLight.enabled = false;
    }

    protected override IEnumerator ShowHideEffect()
    {
        effect.SetActive(true);
        yield return new WaitForSeconds(duration);
        effect.SetActive(false);
    }

    private void OnLight() => TryActivate();
}
