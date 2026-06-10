using UnityEngine;

public class SoldierHealth : MonoBehaviour
{
    [Header("Máu lính")]
    public float maxHealth = 100f;
    public float currentHealth;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return isDead;
    }
}