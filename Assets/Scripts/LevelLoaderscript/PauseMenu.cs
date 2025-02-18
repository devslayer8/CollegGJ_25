using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public ControlsDisplayManager controlsDisplayManager; // Reference to ControlsDisplayManager

    private PlayerMovement[] players;
    private HealerMovement[] healers;

    void Start()
    {
        // Ensure the pause menu is inactive at the start
        pauseMenuUI.SetActive(false);
        players = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);
        healers = Object.FindObjectsByType<HealerMovement>(FindObjectsSortMode.None);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameOverManager.GameIsOver)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Debug.Log("Resume called");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        controlsDisplayManager.SetGamePaused(false);
        TogglePlayerControls(true);
    }

    void Pause()
    {
        Debug.Log("Pause called");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        controlsDisplayManager.SetGamePaused(true);
        TogglePlayerControls(false);
    }

    public void LoadMainMenu()
    {
        Debug.Log("LoadMainMenu called");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevelSelector()
    {
        Debug.Log("LoadLevelSelector called");
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelSelector");
    }

    public void RestartLevel()
    {
        Debug.Log("RestartLevel called");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame called");
        Time.timeScale = 1f;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
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

    public void DisablePauseMenu()
    {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        controlsDisplayManager.SetGamePaused(false); // Ensure controls display is updated when the pause menu is disabled
        TogglePlayerControls(true); // Ensure controls are enabled when the pause menu is disabled
    }
}
