using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityIconUI : MonoBehaviour
{
    [SerializeField] private Image cooldownFill;

    private Coroutine routine;

    public void Bind(ActiveAbility ability)
    {
        ability.CooldownStarted += StartCooldown;
        ability.CooldownFinished += FinishCooldown;
    }

    private void StartCooldown(float duration)
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(CooldownRoutine(duration));
    }

    private IEnumerator CooldownRoutine(float duration)
    {
        float t = 0f;
        cooldownFill.fillAmount = 1f;

        while (t < duration)
        {
            t += Time.deltaTime;
            cooldownFill.fillAmount = 1f - t / duration;
            yield return null;
        }

        cooldownFill.fillAmount = 0f;
    }

    private void FinishCooldown()
    {
        cooldownFill.fillAmount = 0f;
    }
}
