using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
        // --- Config ---
    public float speed = 100;
    public float radiusExplosion = 4.0f;
    public RocketController rocket;
    public LayerMask collisionLayerMask;

        // --- Explosion VFX ---
    public GameObject rocketExplosion;

        // --- Projectile Mesh ---
    public MeshRenderer projectileMesh;

        // --- Script Variables ---
    private bool targetHit;

        // --- Audio ---
    public AudioSource inFlightAudioSource;

        // --- VFX ---
    public ParticleSystem disableOnHit;

    private Camera playerCamera;


    private void Start()
    {
        rocket = GameObject.FindFirstObjectByType<RocketController>();
        playerCamera = Camera.main;
    }


    private void Update()
        {
            if (targetHit) return;
            transform.position += transform.forward * (speed * Time.deltaTime);
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (!enabled)
                return;
            Explode();
            projectileMesh.enabled = false;
            targetHit = true;
            inFlightAudioSource.Stop();
            foreach(Collider col in GetComponents<Collider>())
            {
                col.enabled = false;
            }
            disableOnHit.Stop();
            Destroy(gameObject, 5.0f);
        }

    private void Explode()
    {
        // --- Instantiate new explosion option. I would recommend using an object pool ---
        GameObject newExplosion = Instantiate(rocketExplosion, transform.position, rocketExplosion.transform.rotation, null);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radiusExplosion);

        Dictionary<EnemyBase, int> damageMap = new Dictionary<EnemyBase, int>();
        Dictionary<EnemyBase, Vector3> hitPointMap = new Dictionary<EnemyBase, Vector3>();

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                
                var enemyBase = collider.gameObject.GetComponentInParent<EnemyBase>();
                enemyBase.TakeDamage(rocket.Damage);

                Vector3 hitPoint = collider.ClosestPoint(transform.position);

                if (!damageMap.ContainsKey(enemyBase))
                {
                    damageMap[enemyBase] = 0;
                    hitPointMap[enemyBase] = hitPoint;
                }

                damageMap[enemyBase] += rocket.Damage;                
            } 
        }

        foreach (var pair in damageMap)
        {
            EnemyBase enemy = pair.Key;

            Vector3 hitPoint = hitPointMap[enemy];
            DynamicTextData data = enemy.textData;

            Vector3 destination = hitPoint +
                (playerCamera.transform.position - hitPoint).normalized;

            destination.x += (Random.value - 0.5f) / 3f;
            destination.y += Random.value;
            destination.z += (Random.value - 0.5f) / 3f;

            DynamicTextManager.CreateText(destination, rocket.Damage.ToString(), data);
        }
    }
}