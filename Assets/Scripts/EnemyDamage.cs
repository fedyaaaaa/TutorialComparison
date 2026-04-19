using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private int stompDamage = 1;
    [SerializeField] private float stompBounceForce = 8f;

    [Header("Stomp Check")]
    [SerializeField] private float stompThreshold = 0.1f;

    private Health enemyHealth;

    private void Awake()
    {
        enemyHealth = GetComponent<Health>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        Health playerHealth = collision.gameObject.GetComponent<Health>();
        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (playerHealth == null || playerRb == null)
            return;

        bool hitFromAbove = false;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y <= -stompThreshold)
            {
                hitFromAbove = true;
                break;
            }
        }

        if (hitFromAbove)
        {
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(stompDamage);
            }

            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, stompBounceForce);
        }
        else
        {
            playerHealth.TakeDamage(contactDamage);
        }
    }
}