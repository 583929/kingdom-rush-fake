using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    [Header("Enemies Pool")]
    public GameObject[] enemyPrefabs;

    [Header("Waypoints")]
    public Transform[] waypoints;

    [Header("Spawn Settings")]
    public float timeBetweenSpawn = 1f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
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
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedPrefab = enemyPrefabs[randomIndex];

        if (selectedPrefab == null) return;

        GameObject enemy = Instantiate(selectedPrefab, transform.position, Quaternion.identity);

        // Lấy trực tiếp Script di chuyển trên con quái vừa gọi ra và đổ dữ liệu đường đi vào
        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
        if (movement != null)
        {
            movement.waypoints = waypoints;
            movement.currentState = EnemyMovement.EnemyState.Walk;
        }
    }
}