using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float reachDistance = 0.05f;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private Transform currentTarget;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (pointA == null || pointB == null)
        {
            enabled = false;
            return;
        }

        transform.position = pointA.position;
        currentTarget = pointB;
        UpdateFacingDirection();
    }

    private void FixedUpdate()
    {
        if (currentTarget == null)
            return;

        Vector2 direction = (currentTarget.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        float distanceToTarget = Vector2.Distance(transform.position, currentTarget.position);

        if (distanceToTarget <= reachDistance)
        {
            SwitchTarget();
        }
    }

    private void SwitchTarget()
    {
        if (currentTarget == pointA)
            currentTarget = pointB;
        else
            currentTarget = pointA;

        UpdateFacingDirection();
    }

    private void UpdateFacingDirection()
    {
        if (spriteRenderer == null || currentTarget == null)
            return;

        if (currentTarget.position.x < transform.position.x)
            spriteRenderer.flipX = true;
        else if (currentTarget.position.x > transform.position.x)
            spriteRenderer.flipX = false;
    }

    private void OnDrawGizmos()
    {
        if (pointA != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(pointA.position, 0.15f);
        }

        if (pointB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pointB.position, 0.15f);
        }

        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}