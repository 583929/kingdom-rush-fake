using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    private Vector3 originalScale;
    private Transform enemyTransform;
    public float offsetY = 1.5f;

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
        if (enemyTransform == null && transform.parent != null)
        {
            enemyTransform = transform.parent;
        }

        if (enemyTransform != null)
        {
            transform.rotation = Quaternion.identity;

            Vector3 currentScale = originalScale;
            currentScale.x = originalScale.x * Mathf.Sign(enemyTransform.localScale.x);
            transform.localScale = currentScale;
        }
    }
}