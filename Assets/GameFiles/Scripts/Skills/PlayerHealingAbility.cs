using UnityEngine;
using System.Collections;

public class PlayerHealingAbility : ActiveAbility
{
    [SerializeField] private PlayerHealth health;
    [SerializeField] private GameObject effect;
    private float healAmount;

    protected override float CooldownDuration =>
        IncreaseSkills.Instance.GetHealCooldown();

    private void Start()
    {
        healAmount = IncreaseSkills.Instance.GetHealAmount();
        health = GetComponent<PlayerHealth>();
        effect = GetComponentInChildren<Heal>(true).gameObject;
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

    protected override IEnumerator ShowHideEffect()
    {
        effect.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        effect.SetActive(false);
    }

    private void OnHeal() => TryActivate();
}
