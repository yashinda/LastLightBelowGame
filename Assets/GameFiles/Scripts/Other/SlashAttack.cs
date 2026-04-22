using System.Collections;
using UnityEngine;

public class SlashAttack : MonoBehaviour
{
    public float speed = 10.0f;
    public float damage = 10.0f;
    public float pauseTime = 0.16f;
    private ParticleSystem particleSlash;
    private Transform knight;
    private Transform player;

    private void Awake()
    {
        knight = GameObject.Find("Dark Knight").GetComponent<Transform>();
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Start()
    {
        StartCoroutine(PauseSlash());
        particleSlash = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    private IEnumerator PauseSlash()
    {
        yield return new WaitForSeconds(pauseTime);

        particleSlash.Pause();
    }
}
