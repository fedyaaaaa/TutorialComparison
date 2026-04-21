using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private enum WallSide
    {
        None,
        Left,
        Right
    }

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float crouchMoveSpeed = 2.5f;

    [Header("Wall Jump")]
    [SerializeField] private Transform wallCheckLeft;
    [SerializeField] private Transform wallCheckRight;
    [SerializeField] private float wallCheckRadius = 0.1f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallJumpHorizontalForce = 6f;
    [SerializeField] private float wallJumpVerticalForce = 10f;
    [SerializeField] private float wallJumpLockTime = 0.15f;

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
    private bool isTouchingWallLeft;
    private bool isTouchingWallRight;
    private float moveInput;
    private bool facingRight = true;
    private float wallJumpLockCounter = 0f;

    private WallSide lastWallJumpSide = WallSide.None;

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
        if (Time.timeScale == 0f)
        {
            moveInput = 0f;

            if (isCrouching && CanStandUp())
            {
                SetStandingState();
            }

            return;
        }

        moveInput = 0f;

        if (Input.GetKey(KeyCode.A))
            moveInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            moveInput = 1f;

        if (wallJumpLockCounter > 0f)
            wallJumpLockCounter -= Time.deltaTime;

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        isTouchingWallLeft = wallCheckLeft != null && Physics2D.OverlapCircle(
            wallCheckLeft.position,
            wallCheckRadius,
            wallLayer
        );

        isTouchingWallRight = wallCheckRight != null && Physics2D.OverlapCircle(
            wallCheckRight.position,
            wallCheckRadius,
            wallLayer
        );

        if (isGrounded)
        {
            lastWallJumpSide = WallSide.None;
        }

        if (wallJumpLockCounter <= 0f)
        {
            if (moveInput < 0f)
                facingRight = false;
            else if (moveInput > 0f)
                facingRight = true;
        }

        UpdateFacingVisuals();

        HandleCrouch();

        if (Input.GetKeyDown(KeyCode.Space) && !isCrouching)
        {
            if (CanWallJump())
            {
                PerformWallJump();
            }
            else if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        float currentSpeed = isCrouching ? crouchMoveSpeed : moveSpeed;

        if (wallJumpLockCounter > 0f)
            return;

        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
    }

    private bool CanWallJump()
    {
        if (isGrounded)
            return false;

        if (isTouchingWallLeft && lastWallJumpSide != WallSide.Left)
            return true;

        if (isTouchingWallRight && lastWallJumpSide != WallSide.Right)
            return true;

        return false;
    }

    private void PerformWallJump()
    {
        float horizontalDirection = 0f;

        if (isTouchingWallLeft && lastWallJumpSide != WallSide.Left)
        {
            horizontalDirection = 1f;
            lastWallJumpSide = WallSide.Left;
        }
        else if (isTouchingWallRight && lastWallJumpSide != WallSide.Right)
        {
            horizontalDirection = -1f;
            lastWallJumpSide = WallSide.Right;
        }

        rb.linearVelocity = new Vector2(
            horizontalDirection * wallJumpHorizontalForce,
            wallJumpVerticalForce
        );

        facingRight = horizontalDirection > 0f;
        UpdateFacingVisuals();

        wallJumpLockCounter = wallJumpLockTime;
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

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !facingRight;
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

        if (wallCheckLeft != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(wallCheckLeft.position, wallCheckRadius);
        }

        if (wallCheckRight != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheckRight.position, wallCheckRadius);
        }
    }
}