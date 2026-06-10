using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [Header("TowerData")]
    public TowerData towerData;

    [Header("Target")]
    public float targetAcquisitionInterval = 0.2f;

    protected Transform target;
    protected float shootTimer = 0f;
    protected float targetAcquisitionTimer = 0f;

    protected virtual void Awake()
    {
        towerData = GetComponent<TowerData>();

        if (towerData == null)
        {
            Debug.LogError(gameObject.name + " chưa có TowerData!");
        }
    }

    protected virtual void Update()
    {
        if (towerData == null)
            return;

        targetAcquisitionTimer -= Time.deltaTime;

        if (targetAcquisitionTimer <= 0f)
        {
            FindTarget();
            targetAcquisitionTimer = Mathf.Max(0.01f, targetAcquisitionInterval);
        }

        if (target == null)
            return;

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = Mathf.Max(0.01f, GetShootInterval());
        }
    }

    protected virtual float GetDamage()
    {
        return towerData.damage;
    }

    protected virtual float GetShootInterval()
    {
        return towerData.shootInterval;
    }

    protected virtual void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject enemy = enemies[i];

            if (enemy == null)
                continue;

            float distance = Vector2.Distance(
                transform.position,
                enemy.transform.position
            );

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= towerData.range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    protected virtual void Shoot()
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
                GetDamage(),
                this,
                towerData.projectileSpeed,
                towerData.projectileDuration,
                towerData.projectileSprite
            );
        }

        towerData.PlayShootSound();
    }

    public virtual void ApplyEffect(Transform enemy)
    {
        // Tháp thường không có hiệu ứng
    }
}