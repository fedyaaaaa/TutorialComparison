using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    [SerializeField] private string checkpointID;
    [SerializeField] private Transform respawnPoint;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite inactiveSprite; // red flag
    [SerializeField] private Sprite activeSprite;   // green flag

    private void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (CheckpointSave.HasCheckpointForScene(currentSceneName) &&
            CheckpointSave.checkpointID == checkpointID)
        {
            SetActiveState();
        }
        else
        {
            SetInactiveState();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        string currentSceneName = SceneManager.GetActiveScene().name;

        Vector3 savedPosition = respawnPoint != null
            ? respawnPoint.position
            : transform.position;

        CheckpointSave.SaveCheckpoint(currentSceneName, checkpointID, savedPosition);
        SetActiveState();
    }

    public void SetActiveState()
    {
        if (spriteRenderer != null && activeSprite != null)
            spriteRenderer.sprite = activeSprite;
    }

    public void SetInactiveState()
    {
        if (spriteRenderer != null && inactiveSprite != null)
            spriteRenderer.sprite = inactiveSprite;
    }
}