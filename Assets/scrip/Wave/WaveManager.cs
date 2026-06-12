using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("Cấu Hình Wave")]
    public int currentWave = 0;
    public int totalWaves = 3;
    public int enemiesPerWave = 5;
    public float timeBetweenWaves = 5f;

    [Header("Trạng Thái")]
    public bool isSpawning = false;
    public bool isWaveActive = false;

    void Start()
    {
        StartCoroutine(StartWavesWorkflow());
    }

    IEnumerator StartWavesWorkflow()
    {
        yield return new WaitForSeconds(2f);

        while (currentWave < totalWaves)
        {
            currentWave++;

            if (GameManager.instance != null)
            {
                GameManager.instance.SetWave(currentWave);
            }

            Debug.Log("<color=cyan>--- Bắt đầu Wave: " + currentWave + " ---</color>");

            isSpawning = true;
            isWaveActive = true;

            int enemyCount = enemiesPerWave + (currentWave * 2);

            if (EnemySpawner.instance != null)
            {
                yield return StartCoroutine(EnemySpawner.instance.SpawnWave(enemyCount));
            }

            isSpawning = false;

            while (CountActiveEnemies() > 0)
            {
                yield return new WaitForSeconds(0.5f);
            }

            isWaveActive = false;
            Debug.Log("<color=green>Wave " + currentWave + " đã được dọn sạch!</color>");

            if (currentWave < totalWaves)
            {
                Debug.Log("Chờ " + timeBetweenWaves + " giây để chuẩn bị Wave tiếp theo...");
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        yield return new WaitForSeconds(3f);

        if (GameManager.instance != null)
        {
            GameManager.instance.WinGame();
        }
    }

    int CountActiveEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int aliveCount = 0;

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null) continue;

            EnemyHealth health = enemies[i].GetComponent<EnemyHealth>();
            if (health != null && !health.IsDead())
            {
                aliveCount++;
            }
        }

        return aliveCount;
    }
}