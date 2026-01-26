using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    [SerializeField] private float lifeTime = 2.0f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null) 
                playerHealth.TakeDamage(damage);
        }    
    }
}
