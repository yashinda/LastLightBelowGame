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

        if (Physics.Raycast(spawnBulletTransform.position, shootDirection, out hit, shootingRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                var enemyBase = hit.collider.GetComponentInParent<EnemyBase>();
                if (enemyBase == null)
                    return;

                int finalDamage = Damage;

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Head"))
                    finalDamage *= 2;

                enemyBase.TakeDamage(finalDamage);
            }

            if (hit.collider.CompareTag("Barrel"))
            {
                var barrel = hit.collider.GetComponent<ExplosiveBarrel>();
                if (barrel != null)
                {
                    barrel.Explode();
                }
            }
        }

        Debug.DrawRay(spawnBulletTransform.position, shootDirection * shootingRange, Color.red, 0.3f);
    }
}
