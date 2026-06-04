using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
public class MeleeEnemy : EnemyBase
{
    [SerializeField] private SkinnedMeshRenderer skin;
    [SerializeField] private GameObject axe;
    
    private Rigidbody hitRigid;
    private Vector3 hitDirection;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (!isDeath)
        {
            base.Update();
        }
        else
        {
            animator.SetTrigger("Death");
        }  
    }

    protected override void Patrol()
    {
        if (!reachedPoint)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * patrolDistance;
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolDistance, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                reachedPoint = true;
            }
        }

        if (agent.remainingDistance < 1f && !agent.pathPending)
            reachedPoint = false;

        animator.SetBool("Run", true);

        RotateTowards(agent.steeringTarget);
    }

    protected override void Chase()
    {
        if (CanReachPlayer(out var path))
        {
            agent.SetPath(path);
            animator.SetBool("Run", true);
        }
        else
        {
            OnPlayerUnreachable();
        }
    }

    protected override void Attack()
    {
        if (!isDeath)
        {
            animator.SetBool("Run", false);
            animator.SetTrigger("Attack");
            RotateTowards(player.position);
            isAttacking = true;
            agent.isStopped = true;

            Invoke(nameof(ResetAttack), attackCooldown);
        }
        else
        {
            animator.SetTrigger("Death");
        }

    }

    protected override void Die()
    {
        base.Die();
        animator.SetBool("Run", false);
        animator.SetTrigger("Death");
        
        var skins = GetComponentsInChildren<SkinnedMeshRenderer>();
        if (skins != null)
        {
            foreach (var skin in skins)
            {
                skin.enabled = true;
                var col = skin.GetComponent<Collider>();
                if (col != null)
                    col.enabled = true;
            }
            
            skin.enabled = false;
            axe.SetActive(false);
        }
        
        var rigidbodies = GetComponentsInChildren<Rigidbody>();

        if (rigidbodies != null)
        { 
            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = false;

                Vector3 randomDirection = -hitDirection + Random.insideUnitSphere * 0.35f;

                float randomForce = Random.Range(8f, 12f);

                rb.AddForce(randomDirection.normalized * randomForce, ForceMode.Impulse);

                Vector3 randomTorque = new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f),
                    Random.Range(-20f, 20f));

                rb.AddTorque(randomTorque, ForceMode.Impulse);
            }
        }
    }
    
    public void SetHitData(Rigidbody rb, Vector3 dir)
    {
        hitRigid = rb;
        hitDirection = dir;
    }

    public void DealDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider hit in hitColliders)
        {
            if (hit.gameObject.CompareTag("Player"))
            {
                var playerHealth = hit.gameObject.GetComponent<PlayerHealth>();
                playerHealth.TakeDamage(damage);
            }  
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
        if (currentState == EnemyState.Attack)
            agent.isStopped = false;
    }

    public void PlayFootstep()
    {
        enemySource.pitch = Random.Range(0.9f, 1.05f);
        enemySource.PlayOneShot(stepClip);
    }
}