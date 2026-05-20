using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private int enemyCount = 3;
    [SerializeField] private EnemyEncounter encounter;
    private BoxCollider spawnZone;
    private Transform parent;

    private void Awake()
    {
        spawnZone = GetComponent<BoxCollider>();
        parent = GetComponent<Transform>();
    }

    private void Start()
    {
        SpawnEnemyToPosition();
    }

    public void SpawnEnemyToPosition()
    {
        int i;
        
        for (i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPoint = GetSpawnPoint();
            
            int randomEnemyIndex = Random.Range(0, enemies.Count);
            
            var enemyObj = Instantiate(enemies[randomEnemyIndex], spawnPoint, Quaternion.identity, transform);
            
            EnemyBase enemy = enemyObj.GetComponent<EnemyBase>();
            encounter.RegisterEnemy(enemy);
        }
    }
    
    private Vector3 GetSpawnPoint()
    {
        Bounds bounds = spawnZone.bounds;

        Vector3 randomPoint = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.max.y + 5f,
            Random.Range(bounds.min.z, bounds.max.z)
        );

        RaycastHit[] hits = Physics.RaycastAll(randomPoint, Vector3.down, 50f);

        foreach (var hit in hits)
        {
            if (hit.collider != spawnZone)
                return hit.point;
        }

        return randomPoint;
    }
}
