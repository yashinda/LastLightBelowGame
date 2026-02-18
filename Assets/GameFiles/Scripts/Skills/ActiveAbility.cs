using System;
using System.Collections;
using UnityEngine;

public abstract class ActiveAbility : MonoBehaviour
{
    public event Action<float> CooldownStarted;
    public event Action CooldownFinished;

    protected bool isActive;
    protected bool inCooldown;

    protected abstract float CooldownDuration { get; }
    protected abstract bool CanActivate();
    protected abstract IEnumerator ActivateRoutine();
    protected abstract IEnumerator ShowHideEffect();

    public void TryActivate()
    {
        if (!CanActivate() || isActive || inCooldown)
            return;

        StartCoroutine(AbilityFlow());
        StartCoroutine(ShowHideEffect());
    }

    private IEnumerator AbilityFlow()
    {
        isActive = true;
        yield return ActivateRoutine();
        isActive = false;

        inCooldown = true;
        CooldownStarted?.Invoke(CooldownDuration);

        yield return new WaitForSeconds(CooldownDuration);

        inCooldown = false;
        CooldownFinished?.Invoke();
    }
}
