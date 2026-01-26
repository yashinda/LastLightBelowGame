using Unity.VisualScripting;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosivePartycle;
    [SerializeField] private float explosionDamage;
    [SerializeField] private float explosionRadius;
    [SerializeField] private AudioSource explosionSource;
    [SerializeField] private AudioClip explosionClip;
    private bool hasExplode = false;

    public void Explode()
    {
        if (hasExplode)
            return;

        if (!hasExplode)
        {
            hasExplode = true;
            explosivePartycle.Play();
            explosionSource.PlayOneShot(explosionClip);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider collider in colliders)
            {
                var playerHealth = collider.GetComponent<PlayerHealth>();
                var enemyBase = collider.GetComponentInParent<EnemyBase>();
                var barrel = collider.GetComponent<ExplosiveBarrel>();

                if (playerHealth != null)
                {
                    
                    playerHealth.TakeDamage(explosionDamage);
                }

                if (enemyBase != null)
                {
                    enemyBase.TakeDamage(explosionDamage);
                }

                if (barrel != null)
                {
                    barrel.Explode();
                }
            }
            Destroy(gameObject, explosivePartycle.duration);
        }  
    }
}
