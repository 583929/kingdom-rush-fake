using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Định nghĩa các trạng thái của Quái vật
    public enum EnemyState { Walk, Scan, Run }
    [Header("Trạng Thái Hiện Tại")]
    public EnemyState currentState = EnemyState.Walk;

    [Header("Đường đi (Walk)")]
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    [Header("Tốc độ")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;

    [Header("Quét Tìm Người Chơi (Scan)")]
    public float scanRange = 5f;          // Khoảng cách quét
    public LayerMask playerLayer;         // Layer của Người chơi để tối ưu performance
    public string playerTag = "Player";    // Tag dùng để kiểm tra
    private Transform playerTransform;

    [Header("Tấn Công (Run)")]
    public float attackRange = 1f;         // Vùng nhất định để tấn công
    public int damageToPlayer = 10;        // Sát thương gây ra cho Player
    public float attackCooldown = 1.5f;    // Thời gian hồi chiêu
    private float lastAttackTime;

    [Header("Sát thương khi đi hết đường")]
    public int finalDamage = 1;

    void Update()
    {
        // Luôn cập nhật hướng xoay của Sprite dựa trên hướng di chuyển (ngoại trừ khi đứng yên)
        HandleStateMachine();
    }

    // --- HỆ THỐNG QUẢN LÝ TRẠNG THÁI (STATE MACHINE) ---
    void HandleStateMachine()
    {
        switch (currentState)
        {
            case EnemyState.Walk:
                MoveToWaypoint();
                ScanForPlayer(); // Vừa đi vừa quét tìm người chơi
                break;

            case EnemyState.Scan:
                // Nếu bạn muốn quái đứng yên để quét hoặc thực hiện hiệu ứng quét, xử lý ở đây.
                // Ở đây mặc định vừa đi vừa quét (nằm trong Walk), nếu "Scan" tách riêng, ta có thể gọi:
                ScanForPlayer();
                if (playerTransform == null) currentState = EnemyState.Walk;
                break;

            case EnemyState.Run:
                ChaseAndAttackPlayer();
                break;
        }
    }

    // --- GIAI ĐOẠN 1: WALK (Đi tuần tra giữa các Waypoints) ---
    void MoveToWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex];
        Vector3 direction = target.position - transform.position;

        // Di chuyển với tốc độ walkSpeed
        transform.position = Vector3.MoveTowards(transform.position, target.position, walkSpeed * Time.deltaTime);

        FlipSprite(direction.x);

        // Kiểm tra xem đã đến waypoint chưa
        if ((target.position - transform.position).sqrMagnitude < 0.05f * 0.05f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                ReachEnd();
            }
        }
    }

    // --- GIAI ĐOẠN 2: SCAN (Quét tìm người chơi theo vùng diện rộng) ---
    void ScanForPlayer()
    {
        // Quét một vùng hình cầu xung quanh quái vật (Vùng to)
        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, scanRange, playerLayer);

        if (hitPlayer != null && hitPlayer.CompareTag(playerTag))
        {
            playerTransform = hitPlayer.transform;
            currentState = EnemyState.Run; // Tìm thấy -> Chuyển sang Run
        }
    }

    // --- GIAI ĐOẠN 3: RUN & ATTACK (Đuổi theo và tấn công khi tới vùng nhất định) ---
    void ChaseAndAttackPlayer()
    {
        if (playerTransform == null)
        {
            currentState = EnemyState.Walk; // Mất dấu người chơi -> Quay lại đi tuần
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        Vector3 direction = playerTransform.position - transform.position;

        FlipSprite(direction.x);

        // Nếu nằm ngoài tầm tấn công -> Tiếp tục đuổi theo với tốc độ runSpeed
        if (distanceToPlayer > attackRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, runSpeed * Time.deltaTime);
        }
        else
        {
            // Đã vào vùng nhất định (attackRange) -> Tiến hành tấn công
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }

        // Tùy chọn: Nếu người chơi chạy quá xa tầm quét + 2 đơn vị, quái sẽ bỏ đuổi theo
        if (distanceToPlayer > scanRange + 2f)
        {
            playerTransform = null;
            currentState = EnemyState.Walk;
        }
    }

    void Attack()
    {
        Debug.Log("Quái vật tấn công Người chơi!");
        // Gọi hàm nhận sát thương từ phía Script của Player (Ví dụ: playerTransform.GetComponent<PlayerHealth>().TakeDamage(damageToPlayer);)
    }

    // Đi đến điểm cuối cùng của đường đi
    void ReachEnd()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.TakeDamage(finalDamage);
        }
        Destroy(gameObject);
    }

    // Hàm lật mặt Sprite dựa trên hướng X
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

    // --- XỬ LÝ VA CHẠM (Bắt va chạm gây/nhận sát thương từ Sprite Renderer) ---
    // Lưu ý: Game Object cần có BoxCollider2D/CircleCollider2D đặt Is Trigger = true
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ví dụ 1: Quái chạm vào vũ khí/đạn của Player để NHẬN sát thương
        if (collision.CompareTag("PlayerWeapon"))
        {
            TakeDamage(10); // Hàm quái nhận sát thương
        }

        // Ví dụ 2: Nếu muốn quái gây sát thương trực tiếp khi chạm vào người Player (thay vì dùng tầm Attack ở trên)
        if (collision.CompareTag(playerTag) && currentState == EnemyState.Run)
        {
            Debug.Log("Quái va chạm trực tiếp và gây sát thương cho Player");
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Quái bị mất máu: " + damage);
        // Xử lý trừ máu của quái ở đây, nếu máu <= 0 thì Destroy(gameObject);
    }

    // Vẽ vùng quét trong cửa chọn Scene để bạn dễ căn chỉnh độ rộng (Gizmos)
    private void OnDrawGizmosSelected()
    {
        // Màu đỏ cho vùng tấn công
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Màu vàng cho vùng quét tìm Player
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, scanRange);
    }
}