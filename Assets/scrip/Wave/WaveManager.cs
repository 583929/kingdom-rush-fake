using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int currentWave = 0;
    public int totalWaves = 3;
    public int enemiesPerWave = 5;
    public float timeBetweenWaves = 5f;

    public bool isSpawning = false;

    void Start()
    {
        StartCoroutine(StartWaves());
    }

    IEnumerator StartWaves()
    {
        yield return new WaitForSeconds(2f);

        while (currentWave < totalWaves)
        {
            currentWave++;

            if (GameManager.instance != null)
            {
                GameManager.instance.SetWave(currentWave);
            }

            Debug.Log("Bắt đầu Wave: " + currentWave);

            isSpawning = true;

            int enemyCount = enemiesPerWave + currentWave * 2;

            if (EnemySpawner.instance != null)
            {
                yield return StartCoroutine(EnemySpawner.instance.SpawnWave(enemyCount));
            }

            isSpawning = false;

            yield return new WaitForSeconds(timeBetweenWaves);
        }

        yield return new WaitForSeconds(5f);

        if (GameManager.instance != null)
        {
            GameManager.instance.WinGame();
        }
    }
}