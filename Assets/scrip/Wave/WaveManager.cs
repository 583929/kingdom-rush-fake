using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [Header("Wave Settings")]
    [Range(1, 10)]
    public int currentWave = 0;

    [Range(1, 10)]
    public int totalWaves = 3;

    public int enemiesPerWave = 5;
    public float timeBetweenWaves = 5f;

    public bool isSpawning = false;

    private bool hasStarted = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Không tự chạy wave khi mới vào game
        // Chỉ chạy khi bấm nút Bắt đầu
        currentWave = 0;
    }

    public void BeginWaves()
    {
        if (hasStarted)
            return;

        hasStarted = true;
        StartCoroutine(StartWaves());
    }

    IEnumerator StartWaves()
    {
        yield return new WaitForSeconds(2f);

        while (currentWave < totalWaves)
        {
            if (GameManager.instance != null && GameManager.instance.isGameOver)
            {
                yield break;
            }

            currentWave++;

            if (GameManager.instance != null)
            {
                GameManager.instance.SetWave(currentWave, totalWaves);
            }

            Debug.Log("Bắt đầu Wave: " + currentWave + " / " + totalWaves);

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

        if (GameManager.instance != null && GameManager.instance.isGameOver == false)
        {
            GameManager.instance.WinGame();
        }
    }
}