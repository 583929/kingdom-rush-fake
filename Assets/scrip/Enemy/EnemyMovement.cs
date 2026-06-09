using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum EnemyState { Walk, Scan, Run }

    [Header("--- TRẠNG THÁI HIỆN TẠI ---")]
    public EnemyState currentState = EnemyState.Walk;

    [Header("--- ĐƯỜNG ĐI (WALK STAGE) ---")]
    public Transform[] waypoints;
    public float walkSpeed = 2f;
    private int currentWaypointIndex = 0;

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

    void Start()
    {
        anim = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        FindPlayerRef();
    }

    void Update()
    {
        CheckSpriteCollision();
        HandleStateMachine();
    }

    void HandleStateMachine()
    {
        switch (currentState)
        {
            case EnemyState.Walk:
                MoveToWaypoint();
                ScanForPlayerMath();
                break;

            case EnemyState.Scan:
                ScanForPlayerMath();
                if (playerTransform == null) currentState = EnemyState.Walk;
                break;

            case EnemyState.Run:
                ChargeToPlayerPosition();
                break;
        }
    }

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
        }
    }

    void MoveToWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (anim != null && anim.runtimeAnimatorController != null)
        {
            anim.Play("Enemy_walk");
        }

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
        }
    }

    void Attack()
    {
        isAttacking = true;
        if (anim != null && anim.runtimeAnimatorController != null)
        {
            anim.Play("Enemy_attack");
        }

        float attackDuration = 0.5f;
        Invoke("ResetAttackState", attackDuration);
    }

    void ResetAttackState()
    {
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Quái bị mất máu: " + damage);
    }

    void ReachEnd()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.TakeDamage(finalDamage);
        }
        Destroy(gameObject);
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