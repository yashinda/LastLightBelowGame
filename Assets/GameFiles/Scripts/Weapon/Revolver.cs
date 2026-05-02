using UnityEngine;

public class Revolver : Gun
{
    public Transform drum;
    public float rotationDrumDegrees = 45.0f;
    protected override void Shoot()
    {
        drum.Rotate(0.0f, 0.0f, rotationDrumDegrees);

        RaycastHit hit;

        Ray cameraRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        Vector3 targetPoint;

        if (Physics.Raycast(cameraRay, out RaycastHit cameraHit, shootingRange))
        {
            targetPoint = cameraHit.point;
        }
        else
        {
            targetPoint = cameraRay.origin + cameraRay.direction * shootingRange;
        }

        Vector3 shootDirection = (targetPoint - spawnBulletTransform.position).normalized;

        if (Physics.Raycast(spawnBulletTransform.position, shootDirection, out hit, shootingRange, ~ignoreMask))
        {
            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Knight"))
            {
                Quaternion rot = Quaternion.Euler(-90f, 0f, 0f);
                Instantiate(impactEnemy, hit.point, rot);
            }
            else
            {
                Vector3 dirToPlayer = (spawnBulletTransform.transform.position - hit.point).normalized;
                Vector3 spawnPos = hit.point + dirToPlayer * 0.03f;

                Quaternion rot = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(-90f, 0f, 0f);

                Instantiate(impactHit, spawnPos, rot);
            }

            if (hit.collider.CompareTag("Enemy"))
            {
                var enemyBase = hit.collider.GetComponentInParent<EnemyBase>();
                if (enemyBase == null)
                    return;

                int finalDamage = Damage;

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Head"))
                    finalDamage *= 2;

                enemyBase.TakeDamage(finalDamage);

                DynamicTextData data = enemyBase.textData;

                Vector3 destination = hit.point + (playerCamera.transform.position - hit.point).normalized;

                destination.x += (Random.value - 0.5f) / 3.0f;
                destination.y += Random.value;
                destination.z += (Random.value - 0.5f) / 3.0f;

                DynamicTextManager.CreateText(destination, finalDamage.ToString(), data);
            }

            if (hit.collider.CompareTag("Barrel"))
            {
                var barrel = hit.collider.GetComponent<ExplosiveBarrel>();
                if (barrel != null)
                {
                    barrel.Explode();
                }
            }

            if (hit.collider.CompareTag("Knight"))
            {
                var knight = hit.collider.GetComponentInParent<KnightController>();
                knight.TakeDamage(Damage);
            }
        }

        Debug.DrawRay(spawnBulletTransform.position, shootDirection * shootingRange, Color.red, 0.3f);
    }
}
