using UnityEngine;

public class PlatformStick : MonoBehaviour
{
    private Transform currentPassenger;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // Check if player is landing on top (not from side or below)
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f)
            {
                currentPassenger = collision.transform;
                currentPassenger.SetParent(transform);
                break;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (currentPassenger == collision.transform)
        {
            currentPassenger.SetParent(null);
            currentPassenger = null;
        }
    }
}