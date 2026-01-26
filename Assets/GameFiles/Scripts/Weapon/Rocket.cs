using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float explosionRadius;
    [SerializeField] private RocketLauncher rocket;

    private void Start()
    {
        rocket = GameObject.FindFirstObjectByType<RocketLauncher>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
                collider.gameObject.GetComponentInParent<EnemyBase>().TakeDamage(rocket.Damage);
        }

        Destroy(gameObject);
    }
}
