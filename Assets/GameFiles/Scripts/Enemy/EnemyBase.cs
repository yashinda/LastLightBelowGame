using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public abstract class EnemyBase : MonoBehaviour
{
    public event Action<EnemyBase> OnEnemyDied;
    protected private enum EnemyState { Patrol, Chase, Attack}
    protected private EnemyState currentState;

    [Header("Base Stats")]
    [SerializeField] protected private float maxHealth = 100.0f;
    [SerializeField] protected private float currentHealth;
    [SerializeField] protected private float damage = 10;
    [SerializeField] protected int soulsOnDeath = 50;
    [SerializeField] protected float deathByTime = 0;

    [Header("Patrol & Chase")]
    [SerializeField] protected private float chaseDistance = 25.0f;
    [SerializeField] protected private float patrolDistance = 10.0f;
    [SerializeField] protected private float attackDistance = 1.0f;
    [SerializeField] protected private float moveSpeed = 4.0f;

    [Header("Attack")]
    [SerializeField] protected private float attackRange = 2.0f;
    [SerializeField] protected private float attackCooldown = 2.0f;

    [Header("Audio")]
    [SerializeField] protected private AudioSource enemySource;
    [SerializeField] protected private AudioClip stepClip;
    [SerializeField] protected private AudioClip attackClip;
    [SerializeField, Range(0.9f, 1.1f)] protected float pitchMin = 0.95f;
    [SerializeField, Range(0.9f, 1.1f)] protected float pitchMax = 1.05f;

    [Header("UI")]
    [SerializeField] protected private EnemyHealthBar healthBar;

    protected private NavMeshAgent agent;
    protected private Animator animator;
    protected private bool isDeath = false;
    protected Transform player;
    protected private SvetlesContainer svetlesContainer;
    protected private PlayerHealth playerHealth;
    protected private LevelStateController gameManager;
    protected private bool reachedPoint = false;
    protected private bool isAttacking = false;
    protected private int patrolIndex;
    public bool EnemyIsDead => isDeath;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        player = GameObject.FindWithTag("Player")?.transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        svetlesContainer = FindAnyObjectByType<SvetlesContainer>();
        gameManager = FindAnyObjectByType<LevelStateController>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        if (agent != null)
        {
            agent.speed = moveSpeed;
        }
        currentState = EnemyState.Patrol;
    }

    protected virtual void OnEnable()
    {
        MusicManager.Instance?.RegisterEnemy();
    }

    protected virtual void OnDisable()
    {
        MusicManager.Instance?.UnregisterEnemy();
    }

    protected virtual void Update()
    {
        if (isDeath || player == null || gameManager.IsPaused)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                if (distanceToPlayer < chaseDistance && !playerHealth.PlayerDead && CanReachPlayer(out _))
                    SwitchState(EnemyState.Chase);
                break;

            case EnemyState.Chase:
                Chase();
                if (distanceToPlayer < attackDistance && !playerHealth.PlayerDead)
                    SwitchState(EnemyState.Attack);
                else if (distanceToPlayer > chaseDistance || playerHealth.PlayerDead || !CanReachPlayer(out _))
                    SwitchState(EnemyState.Patrol);
                break;

            case EnemyState.Attack:
                Attack();
                if (distanceToPlayer > attackDistance + 1.0f && !playerHealth.PlayerDead)
                    SwitchState(EnemyState.Chase);
                else if (playerHealth.PlayerDead)
                    SwitchState(EnemyState.Patrol);
                    break;
        }
    }

    public virtual void TakeDamage(float amount)
    {
        if (isDeath)
            return;
           
        currentHealth -= amount;
        healthBar.UpdateBarValue(currentHealth, maxHealth);
        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        isDeath = true;
        Destroy(healthBar.gameObject);

        if (agent != null)
            agent.isStopped = true;

        svetlesContainer?.AddSvetles(soulsOnDeath);
        OnEnemyDied?.Invoke(this);

        Destroy(gameObject, deathByTime);
    }

    private void SwitchState(EnemyState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;

        switch (newState)
        {
            case EnemyState.Patrol:
                agent.isStopped = false;
                reachedPoint = false;
                break;
            case EnemyState.Chase:
                agent.isStopped = false;
                break;
            case EnemyState.Attack:
                agent.isStopped = true;
                break;
        }
    }

    public void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position);
        direction.y = 0;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f);
    }

    protected bool CanReachPlayer(out NavMeshPath path)
    {
        path = new NavMeshPath();

        if (!agent.isOnNavMesh)
            return false;

        return agent.CalculatePath(player.position, path)
               && path.status == NavMeshPathStatus.PathComplete;
    }

    protected virtual void OnPlayerUnreachable()
    {
        SwitchState(EnemyState.Patrol);
    }

    public void PlayAttackSound()
    {
        enemySource.PlayOneShot(attackClip);
    }

    protected abstract void Patrol();
    protected abstract void Chase();
    protected abstract void Attack();
}
