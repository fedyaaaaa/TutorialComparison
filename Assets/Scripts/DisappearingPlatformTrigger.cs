using UnityEngine;

public class DisappearingPlatformTrigger : MonoBehaviour
{
    [SerializeField] private DisappearingPlatform disappearingPlatform;

    private void Awake()
    {
        if (disappearingPlatform == null)
            disappearingPlatform = GetComponentInParent<DisappearingPlatform>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryTrigger(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryTrigger(other);
    }

    private void TryTrigger(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (disappearingPlatform != null)
        {
            disappearingPlatform.TriggerDisappear();
        }
    }
}