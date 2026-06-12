using UnityEngine;

public class BlackSkin_health : MonoBehaviour
{
    [Header("Máu quái")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Tiền thưởng khi chết")]
    public int rewardMoney = 15;

    [Header("UI Thanh Máu")]
    public Component healthSlider;

    private Transform canvasTransform;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.GetType().GetProperty("maxValue")?.SetValue(healthSlider, maxHealth, null);
            healthSlider.GetType().GetProperty("value")?.SetValue(healthSlider, currentHealth, null);

            canvasTransform = healthSlider.transform.parent;
        }
    }

    void Update()
    {
        if (isDead) return;

        if (canvasTransform != null)
        {
            canvasTransform.position = transform.position + new Vector3(0, 1.5f, 0);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.GetType().GetProperty("value")?.SetValue(healthSlider, currentHealth, null);
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

        if (canvasTransform != null)
        {
            Destroy(canvasTransform.gameObject);
        }
    }

    public void DieWithoutReward()
    {
        if (isDead) return;
        isDead = true;

        if (canvasTransform != null)
        {
            Destroy(canvasTransform.gameObject);
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
}