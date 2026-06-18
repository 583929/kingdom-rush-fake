using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Máu quái")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Tiền thưởng khi chết")]
    public int rewardMoney = 15;

    [Header("Cấu hình Thanh Máu")]
    public GameObject healthBarPrefab;
    public Vector3 healthBarOffset = new Vector3(0, 1.5f, 0);

    private HealthBar activeHealthBar;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBarPrefab != null)
        {
            // Sinh ra thanh máu ngay tại vị trí của Spawner / con quái
            GameObject barGo = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);

            // Ép nó làm con của quái ngay lập tức
            barGo.transform.SetParent(transform);

            // Đặt lại tọa độ cục bộ (Local Position) đưa nó về đúng vị trí offset trên đầu quái
            barGo.transform.localPosition = healthBarOffset;

            // Đặt lại góc xoay cục bộ để nó không bị lệch
            barGo.transform.localRotation = Quaternion.identity;

            activeHealthBar = barGo.GetComponent<HealthBar>();
            if (activeHealthBar != null)
            {
                activeHealthBar.SetupHealthBar(maxHealth);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (activeHealthBar != null)
        {
            activeHealthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (Money.instance != null)
        {
            Money.instance.AddMoney(rewardMoney);
        }

        if (activeHealthBar != null)
        {
            Destroy(activeHealthBar.gameObject);
        }
    }

    public void DieWithoutReward()
    {
        if (isDead) return;
        isDead = true;

        if (activeHealthBar != null)
        {
            Destroy(activeHealthBar.gameObject);
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
}