using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RulesTriggerZone : MonoBehaviour
{
    public GameObject rulesCanvas; // Reference to the RulesCanvas
    public Button doneButton; // Reference to the Done button
    public static bool gameIsPaused = false;
    private bool hasTriggered = false; // Flag to check if the effect has already happened

    private PlayerMovement[] players;
    private HealerMovement[] healers;

    void Start()
    {
        // Ensure the rules canvas is inactive at the start
        rulesCanvas.SetActive(false);
        doneButton.onClick.AddListener(ResumeGame);

        players = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);
        healers = Object.FindObjectsByType<HealerMovement>(FindObjectsSortMode.None);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && (other.CompareTag("Player") || other.CompareTag("Healer")))
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        rulesCanvas.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        gameIsPaused = true;
        TogglePlayerControls(false);
    }

    void ResumeGame()
    {
        rulesCanvas.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        gameIsPaused = false;
        TogglePlayerControls(true);
        hasTriggered = true; // Set the flag to true to prevent re-triggering
    }

    private void TogglePlayerControls(bool enabled)
    {
        foreach (var player in players)
        {
            player.enabled = enabled;
        }

        foreach (var healer in healers)
        {
            healer.enabled = enabled;
        }
    }
}
