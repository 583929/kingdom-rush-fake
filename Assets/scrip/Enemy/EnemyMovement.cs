using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Đường đi")]
    public Transform[] waypoints;

    [Header("Tốc độ")]
    public float speed = 2f;

    [Header("Sát thương khi tới đích")]
    public int damageToPlayer = 1;

    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        Transform target = waypoints[currentWaypointIndex];

        Vector3 targetPos = target.position;
        Vector3 currentPos = transform.position;
        Vector3 direction = targetPos - currentPos;

        // move using Vector3 to preserve Z and avoid implicit casts
        transform.position = Vector3.MoveTowards(currentPos, targetPos, speed * Time.deltaTime);

        // flip sprite only when horizontal direction changes sign
        if (Mathf.Abs(direction.x) > 0.001f)
        {
            float sign = Mathf.Sign(direction.x);
            Vector3 scale = transform.localScale;
            if (Mathf.Sign(scale.x) != sign)
            {
                scale.x = Mathf.Abs(scale.x) * sign;
                transform.localScale = scale;
            }
        }

        // use squared distance to avoid sqrt
        float sqrDistance = (targetPos - transform.position).sqrMagnitude;
        const float reachThreshold = 0.05f * 0.05f;

        if (sqrDistance < reachThreshold)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                ReachEnd();
            }
        }
    }

    void ReachEnd()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.TakeDamage(damageToPlayer);
        }

        Destroy(gameObject);
    }
}