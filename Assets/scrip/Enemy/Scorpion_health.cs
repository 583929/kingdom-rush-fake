using UnityEngine;

public class Scorpion_health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Tiền thưởng khi chết")]
    public int rewardMoney = 15;

    [Header("Cấu hình Vị trí Thanh Máu")]
    public GameObject healthBarPrefab;
    public Vector3 healthBarOffset = new Vector3(0f, 5f, 0f);

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
                activeHealthBar.SetupHealthBar(maxHealth);
            }
        }
    }

    void LateUpdate()
    {
        if (isDead || activeHealthBar == null) return;

        activeHealthBar.transform.position = transform.position + healthBarOffset;
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

    public void TakeDamage(int damage)
    {
        TakeDamage((float)damage);
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