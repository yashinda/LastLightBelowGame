using System.Collections;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosivePartycle;
    [SerializeField] private float explosionDamage = 100f;
    [SerializeField] private float explosionRadius = 4f;
    [SerializeField] private AudioSource explosionSource;
    [SerializeField] private AudioClip explosionClip;

    private bool hasExplode = false;
    private float delayExplosion = 0.5f;

    public void Explode(float delay = 0.5f)
    {
        if (hasExplode)
            return;

        StartCoroutine(StartExplode(delay));
    }

    private IEnumerator StartExplode(float delay)
    {
        yield return new WaitForSeconds(delay);

        hasExplode = true;

        explosivePartycle.Play();
        explosionSource.PlayOneShot(explosionClip);

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
            meshRenderer.enabled = false;

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);

            float damage = Mathf.Lerp(explosionDamage * 0.3f, explosionDamage,1f - Mathf.Clamp01(distance / explosionRadius));

            var playerHealth = collider.GetComponentInParent<PlayerHealth>();
            var enemyBase = collider.GetComponentInParent<EnemyBase>();
            var barrel = collider.GetComponent<ExplosiveBarrel>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log(collider.gameObject.name);
                Debug.Log(damage);
            }
            
            if (enemyBase != null)
                enemyBase.TakeDamage(damage);

            if (barrel != null && barrel != this)
                barrel.Explode(delayExplosion);
        }

        Destroy(gameObject, explosivePartycle.duration);
    }
}