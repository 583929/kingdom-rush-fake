using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    [Header("Enemy")]
    public GameObject enemyPrefab;

    [Header("Waypoints")]
    public Transform[] waypoints;

    [Header("Spawn Settings")]
    public float timeBetweenSpawn = 1f;

    // number of currently alive enemies spawned by this spawner
    [HideInInspector]
    public int activeEnemies = 0;

    void Awake()
    {
        instance = this;
    }

    public IEnumerator SpawnWave(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenSpawn);
        }

        // wait until all spawned enemies are gone (dead or reached end)
        yield return new WaitUntil(() => activeEnemies <= 0);
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        // track active enemies
        activeEnemies++;

        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();

        if (movement != null)
        {
            movement.waypoints = waypoints;
        }
    }

    // called by enemies when they die or reach the end
    public void NotifyEnemyRemoved()
    {
        activeEnemies = Mathf.Max(0, activeEnemies - 1);
    }
}