using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialCompleteUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject completePanel;

    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool isComplete = false;

    private void Start()
    {
        Time.timeScale = 1f;

        if (completePanel != null)
            completePanel.SetActive(false);
    }

    public void ShowCompleteScreen()
    {
        if (isComplete)
            return;

        isComplete = true;
        Time.timeScale = 0f;

        if (completePanel != null)
            completePanel.SetActive(true);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;

        // 🔥 CLEAR checkpoint when finishing tutorial
        CheckpointSave.ClearCheckpoint();

        SceneManager.LoadScene(mainMenuSceneName);
    }
}