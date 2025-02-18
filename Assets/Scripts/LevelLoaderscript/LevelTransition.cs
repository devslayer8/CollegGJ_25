using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string nextLevelName;
    public bool requiresWavesCompletion = false;

    private bool wavesCompleted = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Healer"))
        {
            if (requiresWavesCompletion && !wavesCompleted)
            {
                Debug.Log("Complete all waves to proceed to the next level.");
            }
            else
            {
                LoadNextLevel();
            }
        }
    }

    public void CompleteWaves()
    {
        wavesCompleted = true;
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }
}
