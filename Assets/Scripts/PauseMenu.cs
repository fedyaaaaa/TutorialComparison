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
                // If player is dead, ESC should not resume the game
                if (isPlayerDead)
                {
                    if (controlsPanel != null && controlsPanel.activeSelf)
                    {
                        ShowPausePanel();
                    }

                    return;
                }

                // If controls panel is open, ESC returns to pause panel
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