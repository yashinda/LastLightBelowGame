using UnityEngine;

public class Sword : MonoBehaviour
{
    public float damage = 15.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerHealth>();
            player.TakeDamage(damage);
            Debug.Log($"Нанесено урона: {damage}");
        }
    }
}
