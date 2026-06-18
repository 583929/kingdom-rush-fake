using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    private Vector3 originalScale;
    private Transform enemyTransform;
    public float offsetY = 1.5f; // Kho?ng cách thanh máu trên đ?u quái

    void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        originalScale = transform.localScale;
        enemyTransform = transform.parent;
    }

    public void SetupHealthBar(float maxHealth)
    {
        if (slider == null) slider = GetComponentInChildren<Slider>();
        if (slider != null)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }
    }

    public void SetHealth(float currentHealth)
    {
        if (slider != null)
        {
            slider.value = currentHealth;
        }
    }

    void LateUpdate()
    {
        // Theo d?i v? trí c?a Enemy khi di chuy?n
        if (enemyTransform != null)
        {
            transform.position = enemyTransform.position + new Vector3(0, offsetY, 0);
        }

        // Luôn gi? thanh máu hư?ng th?ng đ?ng, không xoay theo quái
        transform.rotation = Quaternion.identity;

        // N?u quái quay m?t (Scale.x b? âm), ép Scale c?a thanh máu ph?i t? s?a l?i đ? không b? l?t ngư?c
        if (enemyTransform != null)
        {
            Vector3 currentScale = originalScale;
            currentScale.x = originalScale.x * Mathf.Sign(enemyTransform.localScale.x);
            transform.localScale = currentScale;
        }
    }
}