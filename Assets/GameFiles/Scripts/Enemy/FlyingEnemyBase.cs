using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class FlyingEnemyBase : MonoBehaviour
{
    public event Action<FlyingEnemyBase> OnEnemyDied;

    [Header("Base Stats")]
    [SerializeField] protected float maxHealth = 50f;
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected int soulsOnDeath = 50;
    [SerializeField] protected float deathByTime = 0f;

    [Header("Target")]
    [SerializeField] protected string playerTag = "Player";

    [Header("Flight")]
    [SerializeField] protected float flySpeed = 7f;
    [SerializeField] protected float rotationSpeed = 8f;

    [Header("Obstacle Avoidance")]
    [Tooltip("����� ����� ��� ����������� �����������")]
    [SerializeField] protected float rayDistance = 1.5f;

    [Tooltip("�������� ������� ����� (� ������) ������������ ������")]
    [SerializeField] protected float raySideOffset = 0.4f;

    [Tooltip("���� ������� ����� (� ��������)")]
    [SerializeField] protected float raySideAngle = 25f;

    [Tooltip("��������� ������ ���������� ����������� �� �����������")]
    [SerializeField] protected float avoidanceStrength = 6f;

    [SerializeField] protected LayerMask obstacleMask;

    protected float currentHealth;
    protected bool isDead;

    protected Transform player;
    protected PlayerHealth playerHealth;
    protected SvetlesContainer svetlesContainer;

    protected Rigidbody rb;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;

        var playerGo = GameObject.FindWithTag(playerTag);
        player = playerGo ? playerGo.transform : null;
        playerHealth = playerGo ? playerGo.GetComponent<PlayerHealth>() : null;

        svetlesContainer = FindAnyObjectByType<SvetlesContainer>();

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    protected virtual void OnEnable()
    {
        MusicManager.Instance?.RegisterEnemy();
    }

    protected virtual void OnDisable()
    {
        MusicManager.Instance?.UnregisterEnemy();
    }

    protected virtual void FixedUpdate()
    {
        if (isDead || player == null || (playerHealth != null && playerHealth.PlayerDead))
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        MoveToPlayerWithAvoidance();
    }

    public virtual void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0f)
            Die();
    }

    protected virtual void Die()
    {
        if (isDead) return;

        isDead = true;
        rb.linearVelocity = Vector3.zero;

        svetlesContainer?.AddSvetles(soulsOnDeath);
        OnEnemyDied?.Invoke(this);

        Destroy(gameObject, deathByTime);
    }

    protected virtual void MoveToPlayerWithAvoidance()
    {
        Vector3 toPlayer = (player.position - transform.position);
        Vector3 desiredDir = toPlayer.sqrMagnitude > 0.0001f ? toPlayer.normalized : transform.forward;

        // 3 ����: �����, �����, ������
        Vector3 origin = transform.position;

        Vector3 fwd = desiredDir;
        Vector3 leftDir = Quaternion.AngleAxis(-raySideAngle, Vector3.up) * fwd;
        Vector3 rightDir = Quaternion.AngleAxis(raySideAngle, Vector3.up) * fwd;

        Vector3 leftOrigin = origin - transform.right * raySideOffset;
        Vector3 rightOrigin = origin + transform.right * raySideOffset;

        bool hitCenter = Physics.Raycast(origin, fwd, out RaycastHit hitC, rayDistance, obstacleMask);
        bool hitLeft = Physics.Raycast(leftOrigin, leftDir, out RaycastHit hitL, rayDistance, obstacleMask);
        bool hitRight = Physics.Raycast(rightOrigin, rightDir, out RaycastHit hitR, rayDistance, obstacleMask);

        Vector3 avoid = Vector3.zero;

        if (hitCenter) avoid += hitC.normal;
        if (hitLeft) avoid += hitL.normal;
        if (hitRight) avoid += hitR.normal;

        Vector3 finalDir = desiredDir;

        if (avoid.sqrMagnitude > 0.0001f)
        {
            // ������ ����������� ����� ����������� �����������
            Vector3 away = avoid.normalized;
            finalDir = Vector3.ProjectOnPlane(desiredDir, away).normalized;

            // ���� ����� �������� ���� � fallback
            if (finalDir.sqrMagnitude < 0.0001f)
                finalDir = Vector3.Cross(away, Vector3.up).normalized;
        }

        rb.linearVelocity = finalDir * flySpeed;

        if (finalDir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(finalDir, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.fixedDeltaTime * rotationSpeed));
        }

#if UNITY_EDITOR
        // ����� �����
        Debug.DrawRay(origin, fwd * rayDistance, hitCenter ? Color.red : Color.green);
        Debug.DrawRay(leftOrigin, leftDir * rayDistance, hitLeft ? Color.red : Color.green);
        Debug.DrawRay(rightOrigin, rightDir * rayDistance, hitRight ? Color.red : Color.green);
#endif
    }
}
