using UnityEngine;
using System.Collections;

public class CreateLight : ActiveAbility
{
    [SerializeField] private Light pointLight;
    [SerializeField] private PlayerHealth playerHealth;

    private float duration;

    protected override float CooldownDuration =>
        IncreaseSkills.Instance.GetLightCooldown();

    private void Start()
    {
        duration = IncreaseSkills.Instance.GetLightDuration();
        playerHealth = GetComponent<PlayerHealth>();
        pointLight = GetComponentInChildren<Light>();
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

    private void OnLight() => TryActivate();
}
