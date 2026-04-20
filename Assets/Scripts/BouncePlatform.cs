using UnityEngine;

public class BouncePlatform : MonoBehaviour
{
    [SerializeField] private float bounceForce = 15f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player touched the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Only bounce if player is falling (prevents weird upward boosts)
                if (rb.linearVelocity.y <= 0f)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
                }
            }
        }
    }
}