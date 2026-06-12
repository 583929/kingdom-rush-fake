using UnityEngine;

public class Scorpion : MonoBehaviour
{
    public enum EnemyState { Walk, Run, Attack, Die }

    [Header("--- TRẠNG THÁI HIỆN TẠI ---")]
    public EnemyState currentState = EnemyState.Walk;

    [Header("--- ĐƯỜNG ĐI (WALK STAGE) ---")]
    public Transform[] waypoints;
    public float walkSpeed = 2f;
    private int currentWaypointIndex = 0;

    [Header("--- QUÉT TÌM LÍNH (2 VÒNG QUÉT) ---")]
    public float scanRange = 5f;
    public float attackRange = 1.2f;
    public string soldierTag = "Soldier";
    private Transform soldierTransform;

    [Header("--- TẤN CÔNG & ĐUỔI THEO (RUN STAGE) ---")]
    public float runSpeed = 4f;
    public int damageToSoldier = 10;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    [Header("--- ĐÍCH ĐẾN ĐƯỜNG ĐI ---")]
    public int finalDamage = 1;

    private Animator anim;
    private bool isAttacking = false;
    private EnemyHealth healthScript;

    void Start()
    {
        anim = GetComponent<Animator>();
        healthScript = GetComponent<EnemyHealth>();
        FindSoldierRef();
    }

    void Update()
    {
        if (currentState == EnemyState.Die) return;

        if (healthScript != null && healthScript.IsDead())
        {
            TriggerDie();
            return;
        }

        HandleStateMachine();
    }

    void HandleStateMachine()
    {
        switch (currentState)
        {
            case EnemyState.Walk:
                if (anim != null) anim.speed = 1f;
                MoveToWaypoint();
                ScanForSoldier();
                break;

            case EnemyState.Run:
                if (anim != null) anim.speed = 1f;
                ChargeToSoldier();
                break;

            case EnemyState.Attack:
                if (anim != null) anim.speed = 1f;
                ExecuteAttackLogic();
                break;
        }
    }

    public void TriggerDie()
    {
        currentState = EnemyState.Die;

        walkSpeed = 0f;
        runSpeed = 0f;

        SafePlayAnimation("die");

        Destroy(gameObject, 2f);
    }

    void FindSoldierRef()
    {
        GameObject[] soldiers = GameObject.FindGameObjectsWithTag(soldierTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestSoldier = null;

        foreach (GameObject soldier in soldiers)
        {
            if (soldier == null) continue;

            SoldierHealth sHealth = soldier.GetComponent<SoldierHealth>();
            if (sHealth != null && sHealth.IsDead()) continue;

            float distance = Vector2.Distance(transform.position, soldier.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestSoldier = soldier;
            }
        }

        if (nearestSoldier != null && shortestDistance <= scanRange)
        {
            soldierTransform = nearestSoldier.transform;
        }
        else
        {
            soldierTransform = null;
        }
    }

    void ScanForSoldier()
    {
        FindSoldierRef();
        if (soldierTransform == null) return;

        float distance = Vector2.Distance(transform.position, soldierTransform.position);
        if (distance <= scanRange)
        {
            if (distance <= attackRange)
            {
                currentState = EnemyState.Attack;
            }
            else
            {
                currentState = EnemyState.Run;
            }
        }
    }

    void MoveToWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        SafePlayAnimation("walk");

        Transform target = waypoints[currentWaypointIndex];
        Vector3 direction = target.position - transform.position;

        transform.position = Vector3.MoveTowards(transform.position, target.position, walkSpeed * Time.deltaTime);
        FlipSprite(direction.x);

        if ((target.position - transform.position).sqrMagnitude < 0.05f * 0.05f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                ReachEnd();
            }
        }
    }

    void ChargeToSoldier()
    {
        if (soldierTransform == null)
        {
            currentState = EnemyState.Walk;
            return;
        }

        SoldierHealth sHealth = soldierTransform.GetComponent<SoldierHealth>();
        if (sHealth != null && sHealth.IsDead())
        {
            soldierTransform = null;
            currentState = EnemyState.Walk;
            return;
        }

        float distance = Vector2.Distance(transform.position, soldierTransform.position);

        if (distance <= attackRange)
        {
            currentState = EnemyState.Attack;
            return;
        }

        if (distance > scanRange)
        {
            soldierTransform = null;
            currentState = EnemyState.Walk;
            return;
        }

        if (!SafePlayAnimation("run"))
        {
            SafePlayAnimation("walk");
        }

        Vector3 direction = soldierTransform.position - transform.position;
        FlipSprite(direction.x);

        transform.position = Vector3.MoveTowards(transform.position, soldierTransform.position, runSpeed * Time.deltaTime);
    }

    void ExecuteAttackLogic()
    {
        if (soldierTransform == null)
        {
            currentState = EnemyState.Walk;
            return;
        }

        SoldierHealth sHealth = soldierTransform.GetComponent<SoldierHealth>();
        if (sHealth != null && sHealth.IsDead())
        {
            soldierTransform = null;
            currentState = EnemyState.Walk;
            return;
        }

        float distance = Vector2.Distance(transform.position, soldierTransform.position);
        if (distance > attackRange)
        {
            currentState = EnemyState.Run;
            return;
        }

        Vector3 dirToSoldier = soldierTransform.position - transform.position;
        FlipSprite(dirToSoldier.x);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
        else if (!isAttacking)
        {
            SafePlayAnimation("walk");
        }
    }

    void Attack()
    {
        isAttacking = true;
        SafePlayAnimation("attack");

        if (soldierTransform != null)
        {
            SoldierHealth sHealth = soldierTransform.GetComponent<SoldierHealth>();
            if (sHealth != null)
            {
                sHealth.TakeDamage(damageToSoldier);
            }
        }

        float attackDuration = 0.5f;
        Invoke("ResetAttackState", attackDuration);
    }

    void ResetAttackState()
    {
        isAttacking = false;
    }

    void ReachEnd()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.TakeDamage(finalDamage);
        }

        if (healthScript != null)
        {
            healthScript.DieWithoutReward();
        }
        else
        {
            Destroy(gameObject);
        }
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

    bool SafePlayAnimation(string stateName)
    {
        if (anim == null || anim.runtimeAnimatorController != null)
        {
            if (anim != null && anim.HasState(0, Animator.StringToHash(stateName)))
            {
                anim.Play(stateName);
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, scanRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}