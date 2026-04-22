using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject controlsPanel;

    [Header("Title Texts")]
    [SerializeField] private GameObject pausedText;
    [SerializeField] private GameObject deathText;
    [SerializeField] private GameObject controlsText;

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

        if (pausedText != null)
            pausedText.SetActive(true);

        if (deathText != null)
            deathText.SetActive(false);

        if (controlsText != null)
            controlsText.SetActive(false);
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

        if (pausedText != null)
            pausedText.SetActive(true);

        if (deathText != null)
            deathText.SetActive(false);

        if (controlsText != null)
            controlsText.SetActive(false);
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

        if (pausedText != null)
            pausedText.SetActive(false);

        if (deathText != null)
            deathText.SetActive(false);

        if (controlsText != null)
            controlsText.SetActive(true);
    }

    public void ShowPausePanel()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);

        if (controlsPanel != null)
            controlsPanel.SetActive(false);

        if (resumeButton != null)
            resumeButton.interactable = !isPlayerDead;

        if (isPlayerDead)
        {
            if (pausedText != null)
                pausedText.SetActive(false);

            if (deathText != null)
                deathText.SetActive(true);

            if (controlsText != null)
                controlsText.SetActive(false);
        }
        else
        {
            if (pausedText != null)
                pausedText.SetActive(true);

            if (deathText != null)
                deathText.SetActive(false);

            if (controlsText != null)
                controlsText.SetActive(false);
        }
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
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

        if (pausedText != null)
            pausedText.SetActive(false);

        if (deathText != null)
            deathText.SetActive(true);

        if (controlsText != null)
            controlsText.SetActive(false);
    }
}