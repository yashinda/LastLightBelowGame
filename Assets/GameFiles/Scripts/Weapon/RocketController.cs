using UnityEngine;

public class RocketController : Gun
{
    [Header("VFX")]
    [SerializeField] private GameObject muzzlePrefab;
    [SerializeField] private Transform muzzlePosition;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject projectileToDisableOnFire;

    [Header("Optional Rotation")]
    [SerializeField] private bool rotate = false;
    [SerializeField] private float rotationSpeed = 0.25f;

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

        // Направление от дула к точке
        Vector3 shootDirection = (targetPoint - muzzlePosition.position).normalized;

        // Поворот ракеты в нужное направление
        Quaternion rotation = Quaternion.LookRotation(shootDirection);

        // Спавн ракеты
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