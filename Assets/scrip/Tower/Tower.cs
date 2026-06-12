using UnityEngine;

public class Tower : TowerBase
{
    protected override void Shoot()
    {
        if (towerData.projectilePrefab == null || towerData.firePoint == null || target == null)
            return;

        GameObject bulletObject = Instantiate(
            towerData.projectilePrefab,
            towerData.firePoint.position,
            Quaternion.identity
        );

        Bullet bullet = bulletObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.SetTarget(
                target,
                (int)GetDamage(),
                this,
                towerData.projectileSpeed,
                towerData.projectileDuration,
                towerData.projectileSprite
            );
        }

        towerData.PlayShootSound();
    }
}