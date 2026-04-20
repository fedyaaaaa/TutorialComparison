using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DisappearingPlatform : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float disappearDelay = 0.5f;

    [Header("References")]
    [SerializeField] private Collider2D platformCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isTriggered = false;

    private void Reset()
    {
        platformCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTriggered)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DisappearRoutine());
        }
    }

    private System.Collections.IEnumerator DisappearRoutine()
    {
        isTriggered = true;

        yield return new WaitForSeconds(disappearDelay);

        if (platformCollider != null)
            platformCollider.enabled = false;

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
    }
}