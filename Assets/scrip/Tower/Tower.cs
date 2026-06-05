using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float range = 3f;
    public float fireRate = 1f;
    public int damage = 25;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    // How often (seconds) the tower will search for a new target. Lower = more responsive, higher = cheaper.
    [Tooltip("Target acquisition interval in seconds")]
    public float targetAcquisitionInterval = 0.2f;

    private float fireCountdown = 0f;
    private float targetAcquisitionTimer = 0f;
    private Transform target;

    void Update()
    {
        // Sample target acquisition at a fixed interval to avoid expensive searches every frame
        targetAcquisitionTimer -= Time.deltaTime;
        if (targetAcquisitionTimer <= 0f)
        {
            AcquireTarget();
            targetAcquisitionTimer = Mathf.Max(0.01f, targetAcquisitionInterval);
        }

        if (target == null)
            return;

        // Ensure fireRate is valid to avoid division by zero
        float clampedFireRate = Mathf.Max(0.0001f, fireRate);

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / clampedFireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    // Find the nearest enemy within range. Uses squared distance for performance.
    void AcquireTarget()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float shortestSqr = float.PositiveInfinity;
        GameObject nearestEnemy = null;
        Vector3 myPos = transform.position;
        float rangeSqr = range * range;

        for (int i = 0; i < enemies.Length; i++)
        {
            var enemy = enemies[i];
            if (enemy == null) continue;

            float sqrDist = (enemy.transform.position - myPos).sqrMagnitude;
            if (sqrDist < shortestSqr)
            {
                shortestSqr = sqrDist;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestSqr <= rangeSqr)
            target = nearestEnemy.transform;
        else
            target = null;
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null || target == null)
            return;

        GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        if (bulletObject == null) return;

        Bullet bullet = bulletObject.GetComponent<Bullet>();
        if (bullet == null) return;

        bullet.SetTarget(target, damage);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void OnValidate()
    {
        range = Mathf.Max(0f, range);
        fireRate = Mathf.Max(0.0001f, fireRate);
        damage = Mathf.Max(0, damage);
        targetAcquisitionInterval = Mathf.Max(0.01f, targetAcquisitionInterval);
    }
}
