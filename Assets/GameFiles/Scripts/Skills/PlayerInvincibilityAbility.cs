using UnityEngine;
using System.Collections;

public class PlayerInvincibilityAbility : ActiveAbility
{
    [SerializeField] private PlayerHealth health;

    protected override float CooldownDuration =>
        IncreaseSkills.Instance.GetInvincibilityCooldown();

    private float duration;

    private void Start()
    {
        duration = IncreaseSkills.Instance.GetInvincibilityDuration();
        health = GetComponent<PlayerHealth>();
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

    private void OnInvincibility() => TryActivate();
}
