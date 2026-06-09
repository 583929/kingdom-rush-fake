using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Máu quái")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Tiền thưởng khi chết")]
    public int rewardMoney = 15;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        TakeDamage((float)damage);
    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;

        if (GameManager.instance != null)
        {
            GameManager.instance.AddMoney(rewardMoney);
        }

        Destroy(gameObject);
    }
}