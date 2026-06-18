using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Stats")]
    public int playerHealth = 20;
    public int money = 100;
    public int currentWave = 0;

    [Range(1, 10)]
    public int totalWave = 10;

    [Header("UI")]
    public TMP_Text healthText;
    public TMP_Text moneyText;
    public TMP_Text waveText;

    [Header("Win / Lose Panel")]
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject finalWavePanel;

    [Header("Game State")]
    public bool isGameOver = false;

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

    void Start()
    {
        isGameOver = false;

        if (winPanel != null)
            winPanel.SetActive(false);

        if (losePanel != null)
            losePanel.SetActive(false);

        if (finalWavePanel != null)
            finalWavePanel.SetActive(false);

        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        if (isGameOver)
            return;

        playerHealth -= damage;

        if (playerHealth < 0)
            playerHealth = 0;

        UpdateUI();

        if (playerHealth <= 0)
        {
            LoseGame();
        }
    }

    public void AddMoney(int amount)
    {
        if (isGameOver)
            return;

        money += amount;
        UpdateUI();
    }

    public bool SpendMoney(int amount)
    {
        if (isGameOver)
            return false;

        if (money >= amount)
        {
            money -= amount;
            UpdateUI();
            return true;
        }

        return false;
    }

    // Hàm này dùng cho WaveManager gọi: SetWave(currentWave, totalWaves)
    public void SetWave(int waveNumber, int maxWave)
    {
        if (isGameOver)
            return;

        currentWave = waveNumber;
        totalWave = Mathf.Clamp(maxWave, 1, 10);

        UpdateUI();
    }

    // Hàm này để tránh lỗi nếu script cũ còn gọi SetWave(currentWave)
    public void SetWave(int waveNumber)
    {
        if (isGameOver)
            return;

        currentWave = waveNumber;
        UpdateUI();
    }

    public void WinGame()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        if (winPanel != null)
            winPanel.SetActive(true);

        if (losePanel != null)
            losePanel.SetActive(false);

        if (finalWavePanel != null)
            finalWavePanel.SetActive(false);

        Debug.Log("YOU WIN");
    }

    public void LoseGame()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        if (losePanel != null)
            losePanel.SetActive(true);

        if (winPanel != null)
            winPanel.SetActive(false);

        if (finalWavePanel != null)
            finalWavePanel.SetActive(false);

        Debug.Log("GAME OVER");
    }

    void UpdateUI()
    {
        if (healthText != null)
            healthText.text = "Máu: " + playerHealth;

        if (moneyText != null)
            moneyText.text = "Tiền: " + money;

        if (waveText != null)
            waveText.text = "Wave: " + currentWave + " / " + totalWave;
    }
}