using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FinishTrigger : MonoBehaviour
{
    [SerializeField] private TutorialCompleteUI tutorialCompleteUI;

    private bool hasTriggered = false;

    private void Awake()
    {
        if (tutorialCompleteUI == null)
            tutorialCompleteUI = FindFirstObjectByType<TutorialCompleteUI>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        hasTriggered = true;

        if (tutorialCompleteUI != null)
            tutorialCompleteUI.ShowCompleteScreen();
    }
}