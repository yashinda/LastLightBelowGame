using UnityEngine;

public class ArrowToQuest : MonoBehaviour
{
    private RectTransform rect;
    private Transform currentTarget;
    private Camera cam;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        cam = Camera.main;
    }

    private void Update()
    {
        UpdateTarget();

        if (currentTarget == null)
            return;

        RotateArrow(currentTarget);
    }

    private void UpdateTarget()
    {
        var encounter = FindAnyObjectByType<EnemyEncounter>();
        if (encounter != null && !encounter.wavesIsStarted)
        {
            currentTarget = encounter.transform;
            return;
        }

        var enemy = FindFirstObjectByType<EnemyBase>();
        if (enemy != null && !enemy.EnemyIsDead)
        {
            currentTarget = enemy.transform;
            return;
        }

        var finishCircle = FindFirstObjectByType<FinishCircle>();
        if (finishCircle != null)
        {
            currentTarget = finishCircle.transform;
            return;
        }

        currentTarget = null;
    }

    private void RotateArrow(Transform target)
    {
        Vector3 toTarget = target.position - cam.transform.position;
        toTarget.y = 0f;

        Vector3 camForward = cam.transform.forward;
        camForward.y = 0f;

        float angle = Vector3.SignedAngle(camForward, toTarget, Vector3.up);
        rect.localRotation = Quaternion.Euler(0f, 0f, -angle);
    }
}
