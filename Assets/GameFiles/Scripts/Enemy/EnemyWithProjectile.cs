using UnityEngine;

public class EnemyWithProjectile : EnemyBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1.0f;
    [SerializeField] private float projectileForce = 1000.0f;
    private float nextFireTime;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (currentState == EnemyState.Attack && Time.time >= nextFireTime)
        {
            Attack();
            nextFireTime = Time.time + fireRate;
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
        RotateTowards(player.position);
        Invoke(nameof(ShootProjectile), fireRate);
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * projectileForce);

        Destroy(projectile, 5.0f);
    }

    private void ResetAttack()
    {
        isAttacking = false;
        if (currentState == EnemyState.Attack)
            agent.isStopped = false;
    }
}