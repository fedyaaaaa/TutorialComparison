using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private int damage = 1;

    [Header("Screen Check")]
    [SerializeField] private float offscreenDestroyDelay = 0.05f;

    private Rigidbody2D rb;
    private float direction = 1f;
    private float offscreenTimer = 0f;
    private bool hasBeenVisible = false;
    private Camera mainCamera;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (mainCamera == null)
            return;

        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

        bool isVisibleOnScreen =
            viewportPos.z > 0f &&
            viewportPos.x >= 0f &&
            viewportPos.x <= 1f &&
            viewportPos.y >= 0f &&
            viewportPos.y <= 1f;

        if (isVisibleOnScreen)
        {
            hasBeenVisible = true;
            offscreenTimer = 0f;
        }
        else if (hasBeenVisible)
        {
            offscreenTimer += Time.unscaledDeltaTime;

            if (offscreenTimer >= offscreenDestroyDelay)
            {
                Destroy(gameObject);
            }
        }
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