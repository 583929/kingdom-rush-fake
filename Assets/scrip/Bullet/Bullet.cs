using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private int damage;

    public float speed = 8f;

    public void SetTarget(Transform enemyTarget, int bulletDamage)
    {
        target = enemyTarget;
        damage = bulletDamage;
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
        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}