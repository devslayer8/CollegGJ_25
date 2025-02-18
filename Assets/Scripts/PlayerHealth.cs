using UnityEngine;
using UnityEngine.SceneManagement;  // Required for reloading the scene

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("Player starting health: " + currentHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player took damage: -" + amount + " HP | Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player Died!");
            Destroy(gameObject);
            ShowGameOverScreen();
        }
    }

    public void Heal(int amount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += amount;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            Debug.Log("Player healed: +" + amount + " HP | Current Health: " + currentHealth);
        }
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
