using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class SkullMonster : EnemyBase
{
    private enum MovementMode
    {
        NavMesh,
        Flying
    }

    [Header("Visuals")]
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private SkinnedMeshRenderer skin;

    [Header("Flying Movement")]
    [SerializeField] private float flySpeed = 8f;
    [SerializeField] private float turnSpeed = 6f;

    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 3.0f;

    [Header("Obstacle Avoidance")]
    [SerializeField] private float rayDistance = 2.5f;
    [SerializeField] private float avoidanceStrength = 3.0f;
    [SerializeField] private LayerMask obstacleMask;

    private NavMeshAgent navAgent;
    private MovementMode movementMode;
    private bool hasExploded;

    protected override void Start()
    {
        base.Start();
        movementMode = MovementMode.NavMesh;
    }

    protected override void Update()
    {
        if (isDeath || player == null)
            return;

        PlayChaseSound();

        bool canReachPlayer = CanReachPlayer(out _);

        if (canReachPlayer)
        {
            if (movementMode != MovementMode.NavMesh)
                SwitchToNavMesh();

            base.Update();
        }
        else
        {
            if (movementMode != MovementMode.Flying)
                SwitchToFlying();

            FlyTowardsPlayer();
            CheckExplosionDistance();
        }
    }

    #region Movement Mode Switching

    private void SwitchToNavMesh()
    {
        movementMode = MovementMode.NavMesh;

        if (navAgent != null)
        {
            navAgent.enabled = true;
            navAgent.isStopped = false;
        }
    }

    private void SwitchToFlying()
    {
        movementMode = MovementMode.Flying;

        if (navAgent != null)
            navAgent.enabled = false;
    }

    #endregion

    #region Flying Movement

    private void FlyTowardsPlayer()
    {
        Vector3 targetPos = player.position + Vector3.up * 0.3f;
        Vector3 targetDir = (targetPos - transform.position).normalized;

        Vector3 avoidance = CalculateAvoidance();
        Vector3 finalDir = (targetDir + avoidance).normalized;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(finalDir),
            turnSpeed * Time.deltaTime
        );

        transform.position += transform.forward * flySpeed * Time.deltaTime;
    }

    private Vector3 CalculateAvoidance()
    {
        Vector3 avoidance = Vector3.zero;

        Vector3[] rayDirections =
        {
            transform.forward,
            Quaternion.Euler(0, 30f, 0) * transform.forward,
            Quaternion.Euler(0, -30f, 0) * transform.forward,
            Quaternion.Euler(30f, 0, 0) * transform.forward,
            Quaternion.Euler(-30f, 0, 0) * transform.forward
        };

        foreach (var dir in rayDirections)
        {
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, rayDistance, obstacleMask))
            {
                avoidance += hit.normal;
            }
        }

        return avoidance * avoidanceStrength;
    }

    #endregion

    #region Explosion

    private void CheckExplosionDistance()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
            Die();
    }

    protected override void Die()
    {
        if (hasExploded)
            return;

        hasExploded = true;

        base.Die();
        Explode();
    }

    private void Explode()
    {
        if (skin != null)
            skin.enabled = false;

        if (explosionParticle != null)
            explosionParticle.Play();

        if (attackClip != null)
            PlayExplosionSound();

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                var playerHealth = collider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                    playerHealth.TakeDamage(damage);
            }
        }
    }

    public void PlayExplosionSound()
    {
        enemySource.PlayOneShot(attackClip);
    }

    public void PlayChaseSound()
    {
        if (isDeath)
            return;

        if (enemySource.isPlaying)
            return;

        enemySource.clip = stepClip;
        enemySource.loop = true;
        enemySource.Play();
    }

    #endregion

    #region EnemyBase FSM (unused)

    protected override void Patrol() { }
    protected override void Chase() 
    {
        

        if (CanReachPlayer(out var path))
        {
            agent.SetPath(path);
            animator.SetBool("Chase", true);
        }
        else
        {
            SwitchToFlying();
        }
    }
    protected override void Attack() 
    {
        Die();
    }

    #endregion
}
