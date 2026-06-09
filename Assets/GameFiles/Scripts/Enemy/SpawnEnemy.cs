using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private int enemyCount = 3;
    [SerializeField] private EnemyEncounter encounter;
    [SerializeField] private LayerMask spawnMask;
    [SerializeField] private GameObject spawnEffect;
    [SerializeField] private float spawnDelay = 2f;
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
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPoint = GetSpawnPoint();

            int randomEnemyIndex = Random.Range(0, enemies.Count);

            StartCoroutine(SpawnEnemyWithEffect(enemies[randomEnemyIndex], spawnPoint));
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

        RaycastHit[] hits = Physics.RaycastAll(randomPoint, Vector3.down, 25f, spawnMask);

        foreach (var hit in hits)
        {
            if (hit.collider != spawnZone)
                return hit.point;
        }

        return randomPoint;
    }
    
    private IEnumerator SpawnEnemyWithEffect(GameObject enemyPrefab, Vector3 spawnPoint)
    {
        GameObject effect = null;

        effect = Instantiate(spawnEffect, spawnPoint, Quaternion.identity, transform);

        yield return new WaitForSeconds(spawnDelay);

        var enemyObj = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity, transform);

        EnemyBase enemy = enemyObj.GetComponent<EnemyBase>();
        encounter.RegisterEnemy(enemy);
    }
}
