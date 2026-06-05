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
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();

        if (enemyAI != null)
        {
            enemyAI.waypoints = waypoints;
            enemyAI.currentState = EnemyAI.EnemyState.Walk;
        }
    }
}