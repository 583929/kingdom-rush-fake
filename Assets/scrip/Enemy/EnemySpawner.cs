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

        // 1. Chọn ngẫu nhiên một vị trí (chỉ số) trong mảng enemyPrefabs
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedPrefab = enemyPrefabs[randomIndex];

        if (selectedPrefab == null) return;

        // 2. Tiến hành Nhân bản (Clone) con quái được chọn ra bản đồ
        GameObject enemy = Instantiate(selectedPrefab, transform.position, Quaternion.identity);

        // 3. Tự động ép đường đi và trạng thái sang cho Script nằm trên quái (Không cần biết tên Script)
        enemy.SendMessage("set_waypoints", waypoints, SendMessageOptions.DontRequireReceiver);
        enemy.SendMessage("set_currentState", EnemyMovement.EnemyState.Walk, SendMessageOptions.DontRequireReceiver);

        // Mẹo phụ: Nếu trong script quái của chú biến waypoints viết thường, dòng dưới này sẽ ép trực tiếp bằng tay
        var targetScript = enemy.GetComponent<MonoBehaviour>();
        if (targetScript != null)
        {
            var field = targetScript.GetType().GetField("waypoints");
            if (field != null) field.SetValue(targetScript, waypoints);
        }
    }
}