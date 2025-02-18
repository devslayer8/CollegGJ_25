using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Add this for TextMeshPro


public class EnemyHealthUI : MonoBehaviour
{
    public TMP_Text healthText;
    private Transform enemyTransform;

    void Start()
    {
        enemyTransform = transform.parent;
    }

    void Update()
    {
        // Keep the health bar above the enemy
        transform.position = enemyTransform.position + new Vector3(0, 0.5f, 0);
    }

    public void UpdateHealth(int currentHealth)
    {
        healthText.text = currentHealth.ToString();

        // Change color to purple if health is below 0
        if (currentHealth < 0)
        {
            healthText.color = Color.magenta;  // Purple color
        }
        else
        {
            healthText.color = Color.white;  // Default color
        }
    }
}
