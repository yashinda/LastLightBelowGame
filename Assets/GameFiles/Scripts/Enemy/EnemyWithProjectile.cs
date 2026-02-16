using UnityEngine;

public class EnemyWithProjectile : EnemyBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1.0f;
    [SerializeField] private float projectileForce = 1000.0f;
    private float nextFireTime;

    protected override void Update()
    {
        if (!isDeath)
        {
            base.Update();
            if (currentState == EnemyState.Attack)
            {
                RotateTowards(player.position);
            }    
        }
        else
        {
            animator.SetTrigger("Death");
        }
        
    }

    protected override void Patrol()
    {
        agent.isStopped = true;
    }

    protected override void Chase()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    protected override void Attack()
    {
        if (isAttacking)
            return;

        isAttacking = true;
        agent.isStopped = true;
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    public void ShootProjectile()
    {
        Quaternion targerRotation = Quaternion.LookRotation(Vector3.right);

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, targerRotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * projectileForce, ForceMode.Impulse);

        Destroy(projectile, 5f);
    }

    private void ResetAttack()
    {
        isAttacking = false;
        if (currentState == EnemyState.Attack)
            agent.isStopped = false;
    }

    protected override void Die()
    {
        base.Die();
        animator.SetTrigger("Death");
    }
}