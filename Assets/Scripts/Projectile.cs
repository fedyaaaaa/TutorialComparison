using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private int damage = 1;

    private Rigidbody2D rb;
    private float direction = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        if (rb == null)
            return;

        if (Time.timeScale == 0f)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = new Vector2(direction * moveSpeed, 0f);
    }

    public void SetDirection(float newDirection)
    {
        direction = Mathf.Sign(newDirection);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore player
        if (other.CompareTag("Player"))
            return;

        // Ignore flagpole
        if (other.CompareTag("flagpole"))
            return;

        // Ignore camera bounds
        if (other.name == "CameraBounds")
            return;

        // Breakable wall
        BreakableWall wall = other.GetComponent<BreakableWall>();
        if (wall != null)
        {
            wall.TakeHit();
            Destroy(gameObject);
            return;
        }

        // Damage enemies (or anything with Health)
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}