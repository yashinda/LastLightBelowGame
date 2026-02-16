using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage = 15.0f;
    [SerializeField] private float lifeTime = 3.0f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (other.CompareTag("Env"))
            Destroy(gameObject);

        var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.TakeDamage(damage);
        Destroy(gameObject);
    }
}