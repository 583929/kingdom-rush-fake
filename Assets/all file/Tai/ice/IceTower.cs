using UnityEngine;

public class IceTower : TowerBase
{
    [Header("Ice Settings")]
    [Tooltip("Mỗi stack làm chậm bao nhiêu. 0.1 = 10%")]
    public float slowPercent = 0.1f;

    [Tooltip("Thời gian hiệu ứng bỏng lạnh")]
    public float frostDuration = 1.5f;

    [Tooltip("Số stack băng tối đa")]
    public int maxIceStack = 5;

    [Tooltip("Sát thương bỏng lạnh mỗi stack. 0.2 = 20% damage tháp")]
    public float frostDamagePercent = 0.2f;

    [Tooltip("Thời gian đóng băng khi đủ stack")]
    public float freezeDuration = 1f;

    public override void ApplyEffect(Transform enemy)
    {
        if (enemy == null)
            return;

        EnemyStatusController status = enemy.GetComponent<EnemyStatusController>();

        if (status != null)
        {
            status.ApplyFrost(
                GetDamage(),
                slowPercent,
                frostDuration,
                maxIceStack,
                frostDamagePercent,
                freezeDuration
            );
        }
    }
}