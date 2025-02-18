using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("StartGame called");
        // Load the main game scene
        SceneManager.LoadScene("Intro_Blockout_A_H");
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame called");
        // Quit the application
        Application.Quit();
#if UNITY_EDITOR
        // Exit play mode in the editor
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void LoadLevelSelector()
    {
        Debug.Log("LoadLevelSelector called");
        // Load the level selector scene
        SceneManager.LoadScene("LevelSelector");
    }
}
