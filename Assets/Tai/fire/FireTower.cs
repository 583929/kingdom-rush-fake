using UnityEngine;

public class FireTower : TowerBase
{
    [Header("Fire Settings")]
    [Tooltip("Tăng damage so với damage gốc trong TowerData. 0.1 = tăng 10%")]
    public float damageBonusPercent = 0.1f;

    [Tooltip("Thời gian thiêu đốt")]
    public float burnDuration = 3f;

    [Tooltip("Số stack lửa tối đa")]
    public int maxFireStack = 5;

    [Tooltip("Mỗi stack gây % damage của tháp. 0.25 = 25%")]
    public float burnDamagePercent = 0.25f;

    protected override float GetDamage()
    {
        if (towerData == null)
            return 0f;

        // Tháp lửa tăng 10% damage so với damage gốc
        return towerData.damage * (1f + damageBonusPercent);
    }

    public override void ApplyEffect(Transform enemy)
    {
        if (enemy == null)
            return;

        EnemyStatusController status = enemy.GetComponent<EnemyStatusController>();

        if (status != null)
        {
            status.ApplyBurn(
                GetDamage(),
                burnDuration,
                maxFireStack,
                burnDamagePercent
            );
        }
    }
}