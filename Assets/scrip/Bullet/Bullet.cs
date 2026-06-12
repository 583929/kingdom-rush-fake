using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private int damage;
    private float speed;
    private float duration;
    private TowerBase towerSource;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetTarget(Transform enemyTarget, int bulletDamage, TowerBase source, float projectileSpeed, float projectileDuration, Sprite projectileSprite)
    {
        target = enemyTarget;
        damage = bulletDamage;
        towerSource = source;
        speed = projectileSpeed;
        duration = projectileDuration;

        if (spriteRenderer != null && projectileSprite != null)
        {
            spriteRenderer.sprite = projectileSprite;
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

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance < 0.1f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        if (target != null)
        {
            EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();

            if (enemyHealth != null && !enemyHealth.IsDead())
            {
                enemyHealth.TakeDamage(damage);

                if (towerSource != null)
                {
                    towerSource.ApplyEffect(target);
                }
            }
        }

        Destroy(gameObject);
    }
}