using UnityEngine;
using System.Collections;

public class Dash : ActiveAbility
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private PlayerHealth playerHealth;

    private float dashDistance;
    private float dashDuration = 0.2f;

    protected override float CooldownDuration =>
        IncreaseSkills.Instance.GetDashCooldown();

    private void Start()
    {
        dashDistance = IncreaseSkills.Instance.GetDashDistance();
        controller = GetComponent<CharacterController>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    protected override bool CanActivate()
    {
        return !playerHealth.PlayerDead;
    }

    protected override IEnumerator ActivateRoutine()
    {
        Vector3 start = transform.position;
        Vector3 target = start + transform.forward * dashDistance;

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            elapsed += Time.deltaTime;
            Vector3 next = Vector3.Lerp(start, target, elapsed / dashDuration);
            controller.Move(next - transform.position);
            yield return null;
        }
    }

    private void OnDash() => TryActivate();
}
