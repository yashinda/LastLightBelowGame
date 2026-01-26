using UnityEngine;
using System.Collections;

public class Evaiding : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CharacterController controller;

    [SerializeField] private float evasionDistance;
    private float evasionDuration = 0.1f;
    [SerializeField] private float cooldownDuration;

    private bool isEvaiding;
    private bool inCooldown;

    private void Start()
    {
        evasionDistance = IncreaseSkills.Instance.GetDashDistance();
        cooldownDuration = IncreaseSkills.Instance.GetDashCooldown();
    }

    private IEnumerator Evasion()
    {
        isEvaiding = true;
        inCooldown = true;

        Vector3 start = playerTransform.position;
        Vector3 target = start + playerTransform.forward * evasionDistance;

        float elapsed = 0f;
        while (elapsed < evasionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / evasionDuration);
            Vector3 nextPos = Vector3.Lerp(start, target, t);
            controller.Move(nextPos - playerTransform.position);
            yield return null;
        }

        isEvaiding = false;
        yield return new WaitForSeconds(cooldownDuration);
        inCooldown = false;
    }

    private void OnEvasion()
    {
        if (!isEvaiding && !inCooldown)
            StartCoroutine(Evasion());
    }
}
