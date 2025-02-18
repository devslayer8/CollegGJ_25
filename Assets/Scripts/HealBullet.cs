using UnityEngine;

public class HealBullet : MonoBehaviour
{
    public int healAmount = 10;  // Amount of health restored
    public LayerMask wallLayer;  // Layer assigned to walls

    public delegate void HealBulletDestroyed(GameObject healBullet);
    public event HealBulletDestroyed OnHealBulletDestroyed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Debug.Log("Heal bullet hit a wall and got destroyed.");
            Destroy(gameObject);
            NotifyDestroyed();
        }
        else if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null && playerHealth.currentHealth < playerHealth.maxHealth)
            {
                int healValue = Mathf.Min(healAmount, playerHealth.maxHealth - playerHealth.currentHealth);
                playerHealth.Heal(healValue);
                Debug.Log("Attacker healed for " + healValue + " HP! Current HP: " + playerHealth.currentHealth);
            }
            Destroy(gameObject);  // Destroy after healing
            NotifyDestroyed();
        }
        else if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && enemy.GetHealth() < 0)
            {
                int healValue = Mathf.Min(healAmount, 0 - enemy.GetHealth());
                enemy.Heal(healValue);
                Debug.Log("Enemy healed for " + healValue + " HP! Current HP: " + enemy.GetHealth());

                if (enemy.GetHealth() == 0)
                {
                    Destroy(enemy.gameObject);
                    Debug.Log("Enemy fully healed and removed!");
                }
            }
            Destroy(gameObject);  // Destroy after healing
            NotifyDestroyed();
        }
    }

    void NotifyDestroyed()
    {
        if (OnHealBulletDestroyed != null)
        {
            OnHealBulletDestroyed(gameObject);
        }
    }
}
