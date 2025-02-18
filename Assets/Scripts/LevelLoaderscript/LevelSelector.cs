using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void LoadLevel1()
    {
        Debug.Log("LoadLevel1 called");
        SceneManager.LoadScene("Intro_Blockout_A_H");
    }

    public void LoadLevel2()
    {
        Debug.Log("LoadLevel2 called");
        SceneManager.LoadScene("Intro_Blockout 1_B");
    }

    public void LoadLevel3()
    {
        Debug.Log("LoadLevel3 called");
        SceneManager.LoadScene("Linearlevel");
    }

    public void LoadLevel4()
    {
        Debug.Log("LoadLevel4 called");
        SceneManager.LoadScene("SemiFinal");
    }

    // Add similar methods for other levels as needed
}
