using UnityEngine;
using UnityEngine.SceneManagement;  // Required for reloading the scene

public class HealerHealth : MonoBehaviour
{
    public int maxHealth = 40;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("Healer starting health: " + currentHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Healer took damage: -" + amount + " HP | Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Healer Died!");
            Destroy(gameObject);
            ShowGameOverScreen();
        }
    }

    public void Heal(int amount, GameObject healer)
    {
        if (healer == gameObject) return; // Prevent self-healing

        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        Debug.Log("Healer healed: +" + amount + " HP | Current Health: " + currentHealth);
    }

    void ShowGameOverScreen()
    {
        GameOverManager gameOverManager = Object.FindFirstObjectByType<GameOverManager>();
        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOverScreen();
        }
    }
}
