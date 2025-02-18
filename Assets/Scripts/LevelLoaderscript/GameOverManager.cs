using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public string levelNameToRestart;
    public static bool GameIsOver = false; // Add this variable
    private PauseMenu pauseMenu; // Reference to PauseMenu script
    public ControlsDisplayManager controlsDisplayManager; // Reference to ControlsDisplayManager

    void Start()
    {
        gameOverPanel.SetActive(false); // Ensure the panel is inactive at the start
        pauseMenu = Object.FindFirstObjectByType<PauseMenu>(); // Find the PauseMenu script
    }

    public void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        GameIsOver = true; // Set the game over state
        controlsDisplayManager.SetGameOver(true);

        // Disable the pause menu
        if (pauseMenu != null)
        {
            pauseMenu.DisablePauseMenu();
        }

        // Stop player and healer movement
        PlayerMovement[] players = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);
        foreach (PlayerMovement player in players)
        {
            player.SetGameOver(true);
        }

        HealerMovement[] healers = Object.FindObjectsByType<HealerMovement>(FindObjectsSortMode.None);
        foreach (HealerMovement healer in healers)
        {
            healer.SetGameOver(true);
        }
    }

    public void RestartLevel()
    {
        Debug.Log("RestartLevel called");
        Time.timeScale = 1f; // Resume the game
        GameIsOver = false; // Reset the game over state
        controlsDisplayManager.SetGameOver(false);
        SceneManager.LoadScene(levelNameToRestart); // Load the specified scene
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame called");
        Time.timeScale = 1f; // Resume the game
        GameIsOver = false; // Reset the game over state
        controlsDisplayManager.SetGameOver(false);
        Application.Quit(); // Quit the application
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Exit play mode in the editor
#endif
    }
}
