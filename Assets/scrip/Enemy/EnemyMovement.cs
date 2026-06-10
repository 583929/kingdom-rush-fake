using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
<<<<<<< Updated upstream
    public enum EnemyState { Walk, Scan, Run }
=======
    public enum EnemyState { Walk, Run, Attack }
>>>>>>> Stashed changes

    [Header("--- TRẠNG THÁI HIỆN TẠI ---")]
    public EnemyState currentState = EnemyState.Walk;

    [Header("--- ĐƯỜNG ĐI (WALK STAGE) ---")]
    public Transform[] waypoints;
    public float walkSpeed = 2f;
    private int currentWaypointIndex = 0;

<<<<<<< Updated upstream
    [Header("--- QUÉT TÌM NGƯỜI CHƠI (SCAN STAGE) ---")]
    public float scanRange = 5f;
    public string playerTag = "Player";
    private Transform playerTransform;
    private SpriteRenderer playerSpriteRenderer;

    [Header("--- TẤN CÔNG & ĐUỔI THEO (RUN STAGE) ---")]
    public float runSpeed = 4f;
    public int damageToPlayer = 10;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    private Vector3 targetRunPosition;
    private bool isCollidingWithPlayer = false;

    [Header("--- ĐÍCH ĐẾN ĐƯỜNG ĐI ---")]
    public int finalDamage = 1;

    private Animator anim;
    private SpriteRenderer mySpriteRenderer;
    private bool isAttacking = false;
=======
    [Header("--- QUÉT TÌM LÍNH (2 VÒNG QUÉT) ---")]
    public float scanRange = 5f;
    public float attackRange = 1.2f; // Đồng bộ tầm đánh cận chiến với lính
    public string soldierTag = "Soldier";
    private Transform soldierTransform;

    [Header("--- TẤN CÔNG & ĐUỔI THEO (RUN STAGE) ---")]
    public float runSpeed = 4f;
    public int damageToSoldier = 10;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    [Header("--- ĐÍCH ĐẾN ĐƯỜNG ĐI ---")]
    public int finalDamage = 1; // KHẮC PHỤC: Đã khai báo lại biến phòng lỗi gạch đỏ dòng 242

    private Animator anim;
    private bool isAttacking = false;
    private EnemyHealth healthScript;
>>>>>>> Stashed changes

    void Start()
    {
        anim = GetComponent<Animator>();
<<<<<<< Updated upstream
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        FindPlayerRef();
=======
        healthScript = GetComponent<EnemyHealth>();
        FindSoldierRef();
>>>>>>> Stashed changes
    }

    void Update()
    {
<<<<<<< Updated upstream
        CheckSpriteCollision();
        HandleStateMachine();
    }

=======
        if (healthScript != null && healthScript.IsDead()) return;

        HandleStateMachine();
    }

>>>>>>> Stashed changes
    void HandleStateMachine()
    {
        switch (currentState)
        {
            case EnemyState.Walk:
<<<<<<< Updated upstream
                MoveToWaypoint();
                ScanForPlayerMath();
                break;

            case EnemyState.Scan:
                ScanForPlayerMath();
                if (playerTransform == null) currentState = EnemyState.Walk;
                break;

            case EnemyState.Run:
                ChargeToPlayerPosition();
=======
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
>>>>>>> Stashed changes
                break;
        }
    }

<<<<<<< Updated upstream
    void FindPlayerRef()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerSpriteRenderer = playerObj.GetComponent<SpriteRenderer>();
        }
    }

    void CheckSpriteCollision()
    {
        if (playerTransform == null || playerSpriteRenderer == null || mySpriteRenderer == null)
        {
            isCollidingWithPlayer = false;
            return;
        }

        Bounds myBounds = mySpriteRenderer.bounds;
        Bounds playerBounds = playerSpriteRenderer.bounds;

        bool dynamicCollision = myBounds.Intersects(playerBounds);

        if (dynamicCollision && !isCollidingWithPlayer)
        {
            isCollidingWithPlayer = true;
            targetRunPosition = transform.position;
        }
        else if (!dynamicCollision && isCollidingWithPlayer)
        {
            isCollidingWithPlayer = false;
            isAttacking = false;
            targetRunPosition = playerTransform.position;
=======
    void FindSoldierRef()
    {
        GameObject[] soldiers = GameObject.FindGameObjectsWithTag(soldierTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestSoldier = null;

        foreach (GameObject soldier in soldiers)
        {
            if (soldier == null) continue;

            // Đồng bộ: Tìm kiếm chính xác component quản lý máu của lính
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
>>>>>>> Stashed changes
        }
    }

    void MoveToWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0) return;
<<<<<<< Updated upstream

        if (anim != null && anim.runtimeAnimatorController != null)
        {
            anim.Play("Enemy_walk");
        }

        Transform target = waypoints[currentWaypointIndex];
        Vector3 direction = target.position - transform.position;

=======

        SafePlayAnimation("Enemy_walk");

        Transform target = waypoints[currentWaypointIndex];
        Vector3 direction = target.position - transform.position;

>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
    void ScanForPlayerMath()
    {
        if (playerTransform == null) FindPlayerRef();
        if (playerTransform == null) return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance <= scanRange)
        {
            targetRunPosition = playerTransform.position;
            currentState = EnemyState.Run;
        }
    }

    void ChargeToPlayerPosition()
    {
        if (isCollidingWithPlayer)
        {
            if (playerTransform != null)
            {
                Vector3 dirToPlayer = playerTransform.position - transform.position;
                FlipSprite(dirToPlayer.x);
            }

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
            else if (!isAttacking)
            {
                if (anim != null && anim.runtimeAnimatorController != null)
                {
                    anim.Play("Enemy_walk");
                }
            }
            return;
        }

        if (playerTransform != null)
        {
            targetRunPosition = playerTransform.position;
        }

        if (anim != null && anim.runtimeAnimatorController != null)
        {
            anim.Play("Enemy_run");
        }

        Vector3 direction = targetRunPosition - transform.position;
        FlipSprite(direction.x);

        transform.position = Vector3.MoveTowards(transform.position, targetRunPosition, runSpeed * Time.deltaTime);

        if ((targetRunPosition - transform.position).sqrMagnitude < 0.05f * 0.05f)
        {
            float distToPlayer = playerTransform != null ? Vector2.Distance(transform.position, playerTransform.position) : 999f;
            if (distToPlayer > scanRange)
            {
                currentState = EnemyState.Walk;
            }
=======
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

      
        if (!SafePlayAnimation("Enemy_run"))
        {
            SafePlayAnimation("Enemy_walk");
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
            SafePlayAnimation("Enemy_walk");
>>>>>>> Stashed changes
        }
    }

    void Attack()
    {
        isAttacking = true;
<<<<<<< Updated upstream
        if (anim != null && anim.runtimeAnimatorController != null)
        {
            anim.Play("Enemy_attack");
=======
        SafePlayAnimation("Enemy_attack");

        if (soldierTransform != null)
        {
            SoldierHealth sHealth = soldierTransform.GetComponent<SoldierHealth>();
            if (sHealth != null)
            {
                sHealth.TakeDamage(damageToSoldier);
            }
>>>>>>> Stashed changes
        }

        float attackDuration = 0.5f;
        Invoke("ResetAttackState", attackDuration);
    }

    void ResetAttackState()
    {
        isAttacking = false;
<<<<<<< Updated upstream
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Quái bị mất máu: " + damage);
=======
>>>>>>> Stashed changes
    }

    void ReachEnd()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.TakeDamage(finalDamage);
        }
<<<<<<< Updated upstream
        Destroy(gameObject);
=======

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
        if (anim == null || anim.runtimeAnimatorController == null) return false;
        if (anim.HasState(0, Animator.StringToHash(stateName)))
        {
            anim.Play(stateName);
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, scanRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
>>>>>>> Stashed changes
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, scanRange);

        if (mySpriteRenderer != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(mySpriteRenderer.bounds.center, mySpriteRenderer.bounds.size);
        }
    }
}