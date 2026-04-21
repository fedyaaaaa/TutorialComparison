using UnityEngine;
using System.Collections;

public class DisappearingPlatform : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float disappearDelay = 0.15f;

    [Header("References")]
    [SerializeField] private Collider2D platformCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isTriggered = false;

    private void Awake()
    {
        if (platformCollider == null)
            platformCollider = GetComponent<Collider2D>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TriggerDisappear()
    {
        if (isTriggered)
            return;

        StartCoroutine(DisappearRoutine());
    }

    private IEnumerator DisappearRoutine()
    {
        isTriggered = true;

        yield return new WaitForSeconds(disappearDelay);

        if (platformCollider != null)
            platformCollider.enabled = false;

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
    }
}