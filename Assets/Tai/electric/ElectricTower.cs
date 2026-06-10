using UnityEngine;

public class ElectricTower : TowerBase
{
    [Header("Electric Settings")]
    [Tooltip("Tăng tốc độ bắn. 0.35 = nhanh hơn 35%")]
    public float fireRateBonusPercent = 0.35f;

    [Tooltip("Giảm damage. 0.1 = giảm 10%")]
    public float damageReducePercent = 0.1f;

    [Header("Chain Lightning")]
    [Tooltip("Khoảng cách giật lan")]
    public float chainRange = 2.5f;

    [Tooltip("Số quái tối đa bị giật lan")]
    public int chainCount = 5;

    [Tooltip("Sát thương giật lan. 0.15 = 15% damage tháp điện")]
    public float chainDamagePercent = 0.15f;

    protected override float GetDamage()
    {
        if (towerData == null)
            return 0f;

        // Tháp điện giảm 10% damage so với damage gốc
        return towerData.damage * (1f - damageReducePercent);
    }

    protected override float GetShootInterval()
    {
        if (towerData == null)
            return 1f;

        // Shoot Interval càng nhỏ thì bắn càng nhanh
        // Nhanh hơn 35% nghĩa là chia cho 1.35
        float fasterRate = 1f + fireRateBonusPercent;

        return towerData.shootInterval / fasterRate;
    }

    public override void ApplyEffect(Transform enemy)
    {
        if (enemy == null)
            return;

        ChainLightning(enemy);
    }

    void ChainLightning(Transform firstEnemy)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        int chained = 0;

        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject enemy = enemies[i];

            if (enemy == null)
                continue;

            // Không giật lại con đã bị đạn bắn trúng
            if (enemy.transform == firstEnemy)
                continue;

            float distance = Vector2.Distance(
                firstEnemy.position,
                enemy.transform.position
            );

            if (distance <= chainRange)
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    float chainDamage = GetDamage() * chainDamagePercent;
                    enemyHealth.TakeDamage((int)chainDamage);
                }

                chained++;

                // Dừng lại khi đã giật đủ 5 quái
                if (chained >= chainCount)
                    break;
            }
        }
    }
}