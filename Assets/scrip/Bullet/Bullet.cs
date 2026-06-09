using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private float damage;
    private TowerBase tower;

    private float speed;
    private float duration;

    private SpriteRenderer spriteRenderer;

    public void SetTarget(
        Transform enemyTarget,
        float bulletDamage,
        TowerBase ownerTower,
        float bulletSpeed,
        float bulletDuration,
        Sprite bulletSprite
    )
    {
        target = enemyTarget;
        damage = bulletDamage;
        tower = ownerTower;
        speed = bulletSpeed;
        duration = bulletDuration;

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && bulletSprite != null)
        {
            spriteRenderer.sprite = bulletSprite;
        }

        Destroy(gameObject, duration);
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        RotateBullet(direction);

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance < 0.1f)
        {
            HitTarget();
        }
    }

    void RotateBullet(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void HitTarget()
    {
        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        if (tower != null)
        {
            tower.ApplyEffect(target);
        }

        Destroy(gameObject);
    }
}