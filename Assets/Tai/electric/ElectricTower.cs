using UnityEngine;

public class ElectricTower : TowerBase
{
    [Header("Electric Settings")]
    public float fireRateBonusPercent = 0.35f;
    public float damageReducePercent = 0.1f;

    [Header("Chain Lightning")]
    public float chainRange = 2.5f;
    public int chainCount = 5;
    public float chainDamagePercent = 0.15f;

    protected override float GetDamage()
    {
        if (towerData == null)
            return 0f;

        return towerData.damage * (1f - damageReducePercent);
    }

    protected override float GetShootInterval()
    {
        if (towerData == null)
            return 1f;

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
        int trackedChained = 0;

        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject enemy = enemies[i];

            if (enemy == null)
                continue;

            if (enemy.transform == firstEnemy)
                continue;

            float distance = Vector2.Distance(
                firstEnemy.position,
                enemy.transform.position
            );

            if (distance <= chainRange)
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

                if (enemyHealth != null && !enemyHealth.IsDead())
                {
                    int chainDamage = (int)(GetDamage() * chainDamagePercent);
                    enemyHealth.TakeDamage(chainDamage);
                }

                trackedChained++;

                if (trackedChained >= chainCount)
                    break;
            }
        }
    }
}