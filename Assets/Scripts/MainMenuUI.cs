using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string interactiveSceneName = "interactive";
    [SerializeField] private string linearSceneName = "linear";

    public void LoadInteractive()
    {
        Time.timeScale = 1f;

        // 🔥 CLEAR checkpoint before starting
        CheckpointSave.ClearCheckpoint();

        SceneManager.LoadScene(interactiveSceneName);
    }

    public void LoadLinear()
    {
        Time.timeScale = 1f;

        // 🔥 CLEAR checkpoint before starting
        CheckpointSave.ClearCheckpoint();

        SceneManager.LoadScene(linearSceneName);
    }
}