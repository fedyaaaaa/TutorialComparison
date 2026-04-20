using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCheckpoint : MonoBehaviour
{
    private void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (CheckpointSave.HasCheckpointForScene(currentSceneName))
        {
            transform.position = CheckpointSave.respawnPosition;
        }
    }
}