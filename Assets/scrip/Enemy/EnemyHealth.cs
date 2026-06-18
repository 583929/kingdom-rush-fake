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
            GameObject barGo = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);

            barGo.transform.SetParent(transform);

            activeHealthBar = barGo.GetComponent<HealthBar>();
            if (activeHealthBar != null)
            {
                // GỌI SANG HEALTHBAR: Thiết lập máu tối đa ban đầu
                activeHealthBar.offsetY = healthBarOffset.y;
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
            // GỌI SANG HEALTHBAR: Cập nhật thanh máu co ngắn lại khi mất máu
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

        Destroy(gameObject);
    }

    public void DieWithoutReward()
    {
        if (isDead) return;
        isDead = true;

        if (activeHealthBar != null)
        {
            Destroy(activeHealthBar.gameObject);
        }

        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return isDead;
    }
}