using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float crouchMoveSpeed = 2.5f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Ceiling Check")]
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private float ceilingCheckRadius = 0.06f;

    [Header("Sprite Swap")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite standingSprite;
    [SerializeField] private Sprite crouchSprite;

    [Header("Standing Collider")]
    [SerializeField] private Vector2 standingColliderSize = new Vector2(0.15f, 0.4f);
    [SerializeField] private Vector2 standingColliderOffset = new Vector2(0f, 0.2f);

    [Header("Crouching Collider")]
    [SerializeField] private Vector2 crouchingColliderSize = new Vector2(0.15f, 0.2f);
    [SerializeField] private Vector2 crouchingColliderOffset = new Vector2(0f, 0.1f);

    [Header("Direction")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform directionIndicator;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private bool isGrounded;
    private bool isCrouching;
    private float moveInput;
    private bool facingRight = true;

    public bool FacingRight => facingRight;
    public Transform FirePoint => firePoint;
    public bool IsCrouching => isCrouching;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        SetStandingState();
        UpdateFacingVisuals();
    }

    private void Update()
    {
        moveInput = 0f;

        if (Input.GetKey(KeyCode.A))
            moveInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            moveInput = 1f;

        if (moveInput < 0f)
            facingRight = false;
        else if (moveInput > 0f)
            facingRight = true;

        UpdateFacingVisuals();

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        HandleCrouch();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouching)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void FixedUpdate()
    {
        float currentSpeed = isCrouching ? crouchMoveSpeed : moveSpeed;
        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
    }

    private void HandleCrouch()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (!isCrouching)
            {
                SetCrouchingState();
            }
        }
        else
        {
            if (isCrouching && CanStandUp())
            {
                SetStandingState();
            }
        }
    }

    private bool CanStandUp()
    {
        if (ceilingCheck == null)
            return true;

        return !Physics2D.OverlapCircle(
            ceilingCheck.position,
            ceilingCheckRadius,
            groundLayer
        );
    }

    private void SetStandingState()
    {
        isCrouching = false;

        if (spriteRenderer != null && standingSprite != null)
            spriteRenderer.sprite = standingSprite;

        if (boxCollider != null)
        {
            boxCollider.size = standingColliderSize;
            boxCollider.offset = standingColliderOffset;
        }
    }

    private void SetCrouchingState()
    {
        isCrouching = true;

        if (spriteRenderer != null && crouchSprite != null)
            spriteRenderer.sprite = crouchSprite;

        if (boxCollider != null)
        {
            boxCollider.size = crouchingColliderSize;
            boxCollider.offset = crouchingColliderOffset;
        }
    }

    private void UpdateFacingVisuals()
    {
        if (firePoint != null)
        {
            Vector3 pos = firePoint.localPosition;
            pos.x = Mathf.Abs(pos.x) * (facingRight ? 1f : -1f);
            firePoint.localPosition = pos;
        }

        if (directionIndicator != null)
        {
            Vector3 pos = directionIndicator.localPosition;
            pos.x = Mathf.Abs(pos.x) * (facingRight ? 1f : -1f);
            directionIndicator.localPosition = pos;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (ceilingCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(ceilingCheck.position, ceilingCheckRadius);
        }
    }
}