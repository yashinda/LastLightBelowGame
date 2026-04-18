using UnityEngine;

public class LightningRotate : MonoBehaviour
{
    public Transform player;
    public AudioSource playerSource;
    public AudioClip impactClip;

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
        }
    }
}
