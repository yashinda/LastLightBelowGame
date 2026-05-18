using UnityEngine;

public class RocketController : Gun
{
    [Header("VFX")]
    [SerializeField] private GameObject muzzlePrefab;
    [SerializeField] private Transform muzzlePosition;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject projectileToDisableOnFire;

    protected override void Shoot()
    {
        Ray cameraRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 targetPoint;

        if (Physics.Raycast(cameraRay, out RaycastHit cameraHit, shootingRange, ~ignoreMask))
        {
            targetPoint = cameraHit.point;
        }
        else
        {
            targetPoint = cameraRay.origin + cameraRay.direction * shootingRange;
        }
        
        Vector3 shootDirection = (targetPoint - muzzlePosition.position).normalized;
        
        Quaternion rotation = Quaternion.LookRotation(shootDirection);
        
        if (projectilePrefab != null && muzzlePosition != null)
        {
            Instantiate(
                projectilePrefab,
                muzzlePosition.position,
                rotation
            );
        }

        if (projectileToDisableOnFire != null)
        {
            projectileToDisableOnFire.SetActive(false);
            Invoke(nameof(ReEnableDisabledProjectile), 3f);
        }
   }

    private void ReEnableDisabledProjectile()
    {
        if (projectileToDisableOnFire != null)
            projectileToDisableOnFire.SetActive(true);
    }
}