using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    [Header("Wave Settings")]
    public int currentWave = 0;
    [Range(1, 100)]
    public int totalWaves = 10;
    [Range(1, 100)]
    public int startingWave = 1;
    public int enemiesPerWave = 5;
    public float timeBetweenWaves = 5f;

    public bool isSpawning = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // If GameManager defines a maxWave, respect it as an upper bound
        if (GameManager.instance != null)
        {
            totalWaves = Mathf.Clamp(totalWaves, 1, GameManager.instance.maxWave);
            startingWave = Mathf.Clamp(startingWave, 1, GameManager.instance.maxWave);
        }
        else
        {
            totalWaves = Mathf.Clamp(totalWaves, 1, 100);
            startingWave = Mathf.Clamp(startingWave, 1, 100);
        }

        // set currentWave to the starting wave (minimum 1)
        currentWave = Mathf.Clamp(startingWave, 1, totalWaves);

        StartCoroutine(StartWaves());
    }

    void OnValidate()
    {
        totalWaves = Mathf.Clamp(totalWaves, 1, 100);
        startingWave = Mathf.Clamp(startingWave, 1, 100);
        currentWave = Mathf.Clamp(currentWave, 0, Mathf.Max(0, totalWaves - 1));
    }

    IEnumerator StartWaves()
    {
        yield return new WaitForSeconds(2f);
        // Run waves from currentWave up to totalWaves (inclusive)
        while (currentWave <= totalWaves)
        {
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

            // if we've reached the last wave, break and win
            if (currentWave >= totalWaves)
                break;

            currentWave++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        yield return new WaitForSeconds(5f);

        if (GameManager.instance != null)
        {
            GameManager.instance.WinGame();
        }
    }
}