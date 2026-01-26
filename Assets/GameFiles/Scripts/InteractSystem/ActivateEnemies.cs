using UnityEngine;

public class ActivateEnemies : MonoBehaviour
{
    [SerializeField] private EnemyEncounter encounter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            encounter.transform.parent = null;
            encounter.StartEncounter();
            Destroy(gameObject);
        }
    }
}
