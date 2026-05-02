using UnityEngine;

public class DeathMagicCircle : MonoBehaviour
{
    public float damage = 15.0f;
    public float lifeTime = 2.0f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Debug.Log("Player hit");
    }
}
