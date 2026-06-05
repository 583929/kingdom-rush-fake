using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Máu quái")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Tiền thưởng khi chết")]
    public int rewardMoney = 15;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.AddMoney(rewardMoney);
        }

        Destroy(gameObject);
    }
}