using System.Collections;
using UnityEngine;

public class Shotgun : Gun
{
    [Header("Shotgun Settings")]
    [SerializeField] private ushort pelletCount = 24;
    [SerializeField] private float spreadAngle = 15.0f;

    protected override void Shoot()
    {
        Ray cameraRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 targetPoint;
        if (Physics.Raycast(cameraRay, out RaycastHit cameraHit, shootingRange))
            targetPoint = cameraHit.point;
        else
            targetPoint = cameraRay.origin + cameraRay.direction * shootingRange;

        Vector3 baseDir = (targetPoint - spawnBulletTransform.position).normalized;

        Vector3 right = Vector3.Cross(Vector3.up, baseDir);
        if (right.sqrMagnitude < 0.0001f) 
            right = Vector3.Cross(playerCamera.transform.right, baseDir);
        right.Normalize();

        Vector3 up = Vector3.Cross(baseDir, right).normalized;

        float spread = Mathf.Tan(spreadAngle * Mathf.Deg2Rad);

        for (int i = 0; i < pelletCount; i++)
        {
            Vector2 rand = Random.insideUnitCircle;
            Vector3 spreadOffset = right * (rand.x * spread) + up * (rand.y * spread);

            Vector3 pelletDir = (baseDir + spreadOffset).normalized;

            if (Physics.Raycast(spawnBulletTransform.position, pelletDir, out RaycastHit hit, shootingRange))
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

            Debug.DrawRay(spawnBulletTransform.position, pelletDir * shootingRange, Color.red, 0.3f);
        }
    }
}
