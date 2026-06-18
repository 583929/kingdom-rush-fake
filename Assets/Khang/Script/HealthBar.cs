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
        // Đề phòng lúc Awake chưa kịp nhận Cha, thì tìm lại ở đây
        if (enemyTransform == null && transform.parent != null)
        {
            enemyTransform = transform.parent;
        }

        if (enemyTransform != null)
        {
            transform.position = enemyTransform.position + new Vector3(0, offsetY, 0);

            // Luôn giữ thanh máu hướng thẳng đứng, không xoay theo quái
            transform.rotation = Quaternion.identity;

            // Nếu quái quay mặt, ép Scale của thanh máu tự sửa lại để không bị lật ngược
            Vector3 currentScale = originalScale;
            currentScale.x = originalScale.x * Mathf.Sign(enemyTransform.localScale.x);
            transform.localScale = currentScale;
        }
    }
}