using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float damage = 15.0f;
    [SerializeField] private float lifeTime = 3.0f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}