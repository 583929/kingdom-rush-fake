using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Máu quái")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Tiền thưởng khi chết")]
    public int rewardMoney = 15;

    [Header("UI Thanh Máu")]
    public Component healthSlider;

    private Transform canvasTransform;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.GetType().GetProperty("maxValue")?.SetValue(healthSlider, (float)maxHealth, null);
            healthSlider.GetType().GetProperty("value")?.SetValue(healthSlider, (float)currentHealth, null);

            // Tự động tìm cụm Canvas cha của Slider
            canvasTransform = healthSlider.transform.parent;
        }
    }

    void Update()
    {
        // CẬP NHẬT: Ép cụm Canvas chứa thanh máu luôn hít chặt theo vị trí con quái + dịch lên trên đầu 1.5 đơn vị
        if (canvasTransform != null)
        {
            canvasTransform.position = transform.position + new Vector3(0, 1.5f, 0);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.GetType().GetProperty("value")?.SetValue(healthSlider, (float)currentHealth, null);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (canvasTransform != null)
        {
            Destroy(canvasTransform.gameObject);
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.AddMoney(rewardMoney);
        }

        Destroy(gameObject);
    }
}