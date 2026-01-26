using UnityEngine;

public class RocketLauncher : Gun
{
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private float rocketForce;
    
    protected override void Shoot()
    {
        GameObject rocket = Instantiate(rocketPrefab, spawnBulletTransform.position, spawnBulletTransform.rotation);

        Rigidbody rb = rocket.GetComponentInParent<Rigidbody>();
        rb.AddForce(spawnBulletTransform.forward * rocketForce);
    }
}
