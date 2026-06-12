using UnityEngine;

public class Linh_Controller : MonoBehaviour
{
    public enum SoldierState { Walk, Attack }

    [Header("--- TRẠNG THÁI HIỆN TẠI ---")]
    public SoldierState currentState = SoldierState.Walk;

    [Header("--- QUÉT TÌM QUÁI (2 VÒNG QUÉT) ---")]
    public float scanRange = 5f;
    public float attackRange = 1.2f;
    public string enemyTag = "Enemy";

    private Transform targetEnemy;

    [Header("--- DI CHUYỂN & TẤN CÔNG ---")]
    public float moveSpeed = 3f;
    public int damageToEnemy = 15;
    public float attackCooldown = 1.2f;
    private float lastAttackTime;

    private bool isAttacking = false;
    private Animator anim;
    private SoldierHealth healthScript;

    void Start()
    {
        anim = GetComponent<Animator>();
        healthScript = GetComponent<SoldierHealth>();
    }

    void Update()
    {
        if (healthScript != null && healthScript.IsDead()) return;

        HandleStateMachine();
    }

    void HandleStateMachine()
    {
        switch (currentState)
        {
            case SoldierState.Walk:
                ScanForEnemy();
                if (targetEnemy == null)
                {
                    // Đồng bộ: Nếu không có mục tiêu, đứng im bằng khung đầu Linh_walk
                    SafePlayAnimation("Linh_walk");
                    if (anim != null) anim.speed = 0f;
                }
                else
                {
                    if (anim != null) anim.speed = 1f;
                    MoveToEnemy();
                }
                break;

            case SoldierState.Attack:
                if (anim != null) anim.speed = 1f;
                ExecuteAttackLogic();
                break;
        }
    }

    void ScanForEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null && enemyHealth.IsDead()) continue;

            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= scanRange)
        {
            targetEnemy = nearestEnemy.transform;

            if (shortestDistance <= attackRange)
            {
                currentState = SoldierState.Attack;
            }
            else
            {
                currentState = SoldierState.Walk;
            }
        }
        else
        {
            targetEnemy = null;
            currentState = SoldierState.Walk;
        }
    }

    void MoveToEnemy()
    {
        if (targetEnemy == null) return;

        EnemyHealth enemyHealth = targetEnemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null && enemyHealth.IsDead())
        {
            targetEnemy = null;
            return;
        }

        float distance = Vector2.Distance(transform.position, targetEnemy.position);
        if (distance <= attackRange)
        {
            currentState = SoldierState.Attack;
            return;
        }
        if (distance > scanRange)
        {
            targetEnemy = null;
            return;
        }

        SafePlayAnimation("Linh_walk");

        Vector3 direction = targetEnemy.position - transform.position;
        FlipSprite(direction.x);

        transform.position = Vector3.MoveTowards(transform.position, targetEnemy.position, moveSpeed * Time.deltaTime);
    }

    void ExecuteAttackLogic()
    {
        if (targetEnemy == null)
        {
            currentState = SoldierState.Walk;
            return;
        }

        EnemyHealth enemyHealth = targetEnemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null && enemyHealth.IsDead())
        {
            targetEnemy = null;
            currentState = SoldierState.Walk;
            return;
        }

        float distance = Vector2.Distance(transform.position, targetEnemy.position);
        if (distance > attackRange)
        {
            currentState = SoldierState.Walk;
            return;
        }

        Vector3 dirToEnemy = targetEnemy.position - transform.position;
        FlipSprite(dirToEnemy.x);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
        else if (!isAttacking)
        {
            // Trong thời gian chờ hồi chiêu, lính đứng im nhìn quái
            SafePlayAnimation("Linh_walk");
            if (anim != null) anim.speed = 0f;
        }
    }

    void Attack()
    {
        isAttacking = true;
        SafePlayAnimation("Linh_attack");

        if (targetEnemy != null)
        {
            EnemyHealth enemyHealth = targetEnemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageToEnemy);
            }
        }

        float attackDuration = 0.5f;
        Invoke("ResetAttackState", attackDuration);
    }

    void ResetAttackState()
    {
        isAttacking = false;
    }

    void FlipSprite(float directionX)
    {
        if (Mathf.Abs(directionX) > 0.001f)
        {
            float sign = Mathf.Sign(directionX);
            Vector3 scale = transform.localScale;
            if (Mathf.Sign(scale.x) != sign)
            {
                scale.x = Mathf.Abs(scale.x) * sign;
                transform.localScale = scale;
            }
        }
    }

    void SafePlayAnimation(string stateName)
    {
        if (anim == null || anim.runtimeAnimatorController == null) return;
        if (anim.HasState(0, Animator.StringToHash(stateName)))
        {
            anim.Play(stateName);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, scanRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}