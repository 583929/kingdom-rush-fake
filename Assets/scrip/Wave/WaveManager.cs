using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("Cấu Hình Wave")]
    public int currentWave = 0;
    public int totalWaves = 3;
    public int enemiesPerWave = 5;
    public float timeBetweenWaves = 5f; // Thời gian chờ GIỮA các đợt khi đã dọn sạch quái

    [Header("Trạng Thái")]
    public bool isSpawning = false;
    public bool isWaveActive = false;

    void Start()
    {
        StartCoroutine(StartWavesWorkflow());
    }

    IEnumerator StartWavesWorkflow()
    {
        // Chờ 2 giây đầu game trước khi bắt đầu đợt 1
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

            // Công thức tính số lượng quái tăng dần theo từng Wave
            int enemyCount = enemiesPerWave + (currentWave * 2);

            // Gọi Spawner sinh quái ra
            if (EnemySpawner.instance != null)
            {
                yield return StartCoroutine(EnemySpawner.instance.SpawnWave(enemyCount));
            }

            isSpawning = false;

            // Chờ cho đến khi người chơi tiêu diệt HẾT quái vật trên bản đồ hoặc chúng đi tới đích tự hủy
            // Đảm bảo quái vật được gán Tag là "Enemy" trong Unity Editor
            while (CountActiveEnemies() > 0)
            {
                yield return new WaitForSeconds(0.5f); // Kiểm tra lại sau mỗi 0.5 giây để tránh nặng máy
            }

            isWaveActive = false;
            Debug.Log("<color=green>Wave " + currentWave + " đã được dọn sạch!</color>");

            // Nếu chưa phải Wave cuối cùng thì chờ một khoảng thời gian nghỉ rồi mới sang Wave kế tiếp
            if (currentWave < totalWaves)
            {
                Debug.Log("Chờ " + timeBetweenWaves + " giây để chuẩn bị Wave tiếp theo...");
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        // Sau khi hoàn thành Wave cuối cùng và dọn sạch quái, chờ 3 giây rồi thắng game
        yield return new WaitForSeconds(3f);

        if (GameManager.instance != null)
        {
            GameManager.instance.WinGame();
        }
    }

    /// <summary>
    /// Hàm đếm số lượng quái vật hiện tại đang có trên map dựa vào Tag "Enemy"
    /// </summary>
    int CountActiveEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length;
    }
}