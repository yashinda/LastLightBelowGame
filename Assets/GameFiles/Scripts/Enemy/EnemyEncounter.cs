using System.Collections.Generic;
using UnityEngine;

public class EnemyEncounter : MonoBehaviour
{
    [SerializeField] private List<GameObject> waves = new();
    private int currentWaveIndex = -1;
    private int aliveEnemies;
    public GameObject nextEncounter;
    public bool wavesIsStarted = false;

    public void StartEncounter()
    {
        ActivateNextWave();
        wavesIsStarted = true;
    }

    private void ActivateNextWave()
    {
        currentWaveIndex++;

        if (currentWaveIndex >= waves.Count)
        {
            OnEncounterCompleted();
            return;
        }

        GameObject wave = waves[currentWaveIndex];
        wave.SetActive(true);

        EnemyBase[] enemies = wave.GetComponentsInChildren<EnemyBase>();
        aliveEnemies = enemies.Length;

        foreach (var enemy in enemies)
        {
            enemy.OnEnemyDied += HandleEnemyDeath;
        }
    }

    private void HandleEnemyDeath(EnemyBase enemy)
    {
        enemy.OnEnemyDied -= HandleEnemyDeath;
        aliveEnemies--;

        if (aliveEnemies <= 0)
        {
            ActivateNextWave();
        }
    }

    private void OnEncounterCompleted()
    {
        if (nextEncounter != null)
            nextEncounter.SetActive(true);
        Destroy(gameObject);
    }
}
