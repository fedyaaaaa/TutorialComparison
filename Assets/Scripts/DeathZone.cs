using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Health health = other.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(health.CurrentHealth);
        }
    }
}