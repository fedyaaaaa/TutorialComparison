using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject controlsPanel;

    [Header("Buttons")]
    [SerializeField] private Button resumeButton;

    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool isPaused = false;
    private bool isPlayerDead = false;

    private void Start()
    {
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (controlsPanel != null)
            controlsPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                if (isPlayerDead)
                {
                    if (controlsPanel != null && controlsPanel.activeSelf)
                    {
                        ShowPausePanel();
                    }
                    return;
                }

                if (controlsPanel != null && controlsPanel.activeSelf)
                {
                    ShowPausePanel();
                }
                else
                {
                    ResumeGame();
                }
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        if (controlsPanel != null)
            controlsPanel.SetActive(false);

        if (resumeButton != null)
            resumeButton.interactable = !isPlayerDead;
    }

    public void ResumeGame()
    {
        if (isPlayerDead)
            return;

        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (controlsPanel != null)
            controlsPanel.SetActive(false);
    }

    public void ShowControls()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (controlsPanel != null)
            controlsPanel.SetActive(true);
    }

    public void ShowPausePanel()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);

        if (controlsPanel != null)
            controlsPanel.SetActive(false);

        if (resumeButton != null)
            resumeButton.interactable = !isPlayerDead;
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 🟢 NEW FUNCTION
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        // Optional but recommended (prevents checkpoint carry-over)
        CheckpointSave.ClearCheckpoint();

        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void HandlePlayerDeath()
    {
        isPlayerDead = true;
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        if (controlsPanel != null)
            controlsPanel.SetActive(false);

        if (resumeButton != null)
            resumeButton.interactable = false;
    }
}