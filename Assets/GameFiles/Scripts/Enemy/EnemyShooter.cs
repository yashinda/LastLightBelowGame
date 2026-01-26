using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyShooter : EnemyBase
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 25f;
    [SerializeField] private float fireRate = 1f;
    //[SerializeField] private AudioClip shootSound;
    //[SerializeField] private ParticleSystem muzzleFlash;

    private float nextFireTime = 0f;

    protected override void Start()
    {
        base.Start();
        agent.updateRotation = false;
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

        RotateTowards(agent.steeringTarget);
    }

    protected override void Chase()
    {
        if (player != null)
            agent.SetDestination(player.position);
    }

    protected override void Attack()
    {
        if (player == null)
            return;

        RotateTowards(player.position);

        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        if (bulletPrefab && firePoint)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb)
                rb.linearVelocity = firePoint.forward * bulletSpeed;

            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript)
                bulletScript.SetDamage(damage);
        }

        yield return null;
    }
}
