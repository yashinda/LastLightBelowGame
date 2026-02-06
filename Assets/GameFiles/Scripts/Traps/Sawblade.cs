using UnityEngine;

public class Sawblade : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10.0f;
    [SerializeField] private float damage = 15.0f;

    private void Update()
    {
        transform.Rotate(0.0f, 0.0f, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
                playerHealth.TakeDamage(damage * Time.deltaTime);
        }

        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponentInParent<EnemyBase>();

            if (enemy != null)
                enemy.TakeDamage(damage * Time.deltaTime);
        }
    }
}
