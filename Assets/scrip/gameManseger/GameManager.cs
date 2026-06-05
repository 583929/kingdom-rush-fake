using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Stats")]
    public int playerHealth = 20;
    public int money = 100;
    public int currentWave = 1;
    [Header("Gameplay")]
    [SerializeField] private int maxWave = 10;

    [Header("UI")]
    public TMP_Text healthText;
    public TMP_Text moneyText;
    public TMP_Text waveText;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject finalWavePanel;

    [Header("Game State")]
    public bool isGameOver = false;
    private bool finalWaveReached = false;

    public event Action<int> OnWaveCompleted;

    void Awake()
    {
        // simple singleton guard to avoid multiple managers
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
        currentWave = Mathf.Clamp(currentWave, 1, maxWave);
        UpdateUI();

        if (winPanel != null)
            winPanel.SetActive(false);

        if (losePanel != null)
            losePanel.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (isGameOver)
            return;

        playerHealth -= damage;
        playerHealth = Mathf.Max(playerHealth, 0);

        UpdateUI();

        if (playerHealth <= 0)
            LoseGame();
    }

    public void AddMoney(int amount)
    {
        if (isGameOver == true)
            return;

        money += amount;
        UpdateUI();
    }

    public bool SpendMoney(int amount)
    {
        if (isGameOver)
            return false;

        if (money < amount)
            return false;

        money -= amount;
        UpdateUI();
        return true;
    }

    public void SetWave(int waveNumber)
    {
        if (isGameOver)
            return;

        currentWave = Mathf.Clamp(waveNumber, 1, maxWave);
        UpdateUI();

        // fire wave completed event for listeners
        OnWaveCompleted?.Invoke(currentWave);

        if (currentWave >= maxWave && !finalWaveReached)
        {
            // mark final wave reached and show finalWavePanel instead of immediate win
            finalWaveReached = true;
            if (finalWavePanel != null)
                finalWavePanel.SetActive(true);
        }
    }

    // Advance to the next wave (external systems should call this when ready)
    public void NextWave()
    {
        if (isGameOver)
            return;

        if (currentWave < maxWave)
        {
            SetWave(currentWave + 1);
        }
        else if (currentWave >= maxWave && finalWaveReached)
        {
            // already at final wave; complete it and win
            CompleteFinalWaveAndWin();
        }
    }

    // Call this to finalize the final wave and trigger win
    public void CompleteFinalWaveAndWin()
    {
        if (isGameOver)
            return;

        finalWaveReached = false;
        if (finalWavePanel != null)
            finalWavePanel.SetActive(false);

        WinGame();
    }

    public void WinGame()
    {
        isGameOver = true;

        if (winPanel != null)
            winPanel.SetActive(true);

        Debug.Log("You Win!");
    }

    public void LoseGame()
    {
        isGameOver = true;

        if (losePanel != null)
            losePanel.SetActive(true);

        Debug.Log("Game Over!");
    }

    void UpdateUI()
    {
        if (healthText != null)
            healthText.text = "Máu: " + playerHealth;

        if (moneyText != null)
            moneyText.text = "Tiền: " + money;

        if (waveText != null)
            waveText.text = $"Wave: {currentWave}/{maxWave}";
    }
}