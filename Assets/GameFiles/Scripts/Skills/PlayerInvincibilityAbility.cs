using UnityEngine;
using System.Collections;

public class PlayerInvincibilityAbility : ActiveAbility
{
    [SerializeField] private PlayerHealth health;
    [SerializeField] private GameObject effect;

    protected override float CooldownDuration =>
        IncreaseSkills.Instance.GetInvincibilityCooldown();

    private float duration;

    private void Start()
    {
        duration = IncreaseSkills.Instance.GetInvincibilityDuration();
        health = GetComponent<PlayerHealth>();
        effect = GetComponentInChildren<Shield>(true).gameObject;
    }

    protected override bool CanActivate()
    {
        return !health.PlayerDead;
    }

    protected override IEnumerator ActivateRoutine()
    {
        health.SetInvincible(true);
        yield return new WaitForSeconds(duration);
        health.SetInvincible(false);
    }

    protected override IEnumerator ShowHideEffect()
    {
        effect.SetActive(true);
        yield return new WaitForSeconds(duration);
        effect.SetActive(false);
    }

    private void OnInvincibility() => TryActivate();
}
