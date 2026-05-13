using System.Collections;
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
    private float delayExplosion = 0.5f;
    private Light explosiveLight;

    private void Start()
    {
        explosiveLight = GetComponentInChildren<Light>(true);
    }

    public void Explode(float delay = 0.5f)
    {
        if (hasExplode)
            return;

        if (!hasExplode)
        {
            StartCoroutine(StartExplode(delay));
        }  
    }

    private IEnumerator StartExplode(float delay)
    {
        explosiveLight.gameObject.SetActive(true);
        yield return new WaitForSeconds(delayExplosion);
        
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
                barrel.Explode(delay + delayExplosion);
            }
        }
        Destroy(gameObject, explosivePartycle.duration);
    }
}
