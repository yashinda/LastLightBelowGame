using UnityEngine;
using System.Collections;

public class PlayerHealingAbility : ActiveAbility
{
    [SerializeField] private PlayerHealth health;
    private float healAmount;

    protected override float CooldownDuration =>
        IncreaseSkills.Instance.GetHealCooldown();

    private void Start()
    {
        healAmount = IncreaseSkills.Instance.GetHealAmount();
        health = GetComponent<PlayerHealth>();
    }

    protected override bool CanActivate()
    {
        return !health.PlayerDead;
    }

    protected override IEnumerator ActivateRoutine()
    {
        health.Heal(healAmount);
        yield break;
    }

    private void OnHeal() => TryActivate();
}
