using UnityEngine;

public class WoodSpikes : MonoBehaviour
{
    public float damage = 10.0f;
    public float stunDuration = 1.5f;

    private void Start()
    {
        Destroy(gameObject, stunDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Stun(stunDuration);
            }

            var playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
