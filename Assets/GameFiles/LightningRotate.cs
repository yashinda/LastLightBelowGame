using UnityEngine;

public class LightningRotate : MonoBehaviour
{
    public Transform player;
    public AudioSource playerSource;
    public AudioClip impactClip;
    public float stunDuration = 3.0f;
    public float damage = 10.0f;

    void Update()
    {
        transform.LookAt(player.position);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Попадание в игрока");
            playerSource.PlayOneShot(impactClip);
            var playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Stun(stunDuration);
            }
            var playerHealth = player.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(10.0f);
        }
    }
}
