using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public Image attackerHealthBarFill;
    public Image healerHealthBarFill;

    private PlayerHealth attackerHealth;
    private HealerHealth healerHealth;

    void Start()
    {
        // Find the Attacker and Healer GameObjects
        GameObject attacker = GameObject.FindGameObjectWithTag("Player");
        GameObject healer = GameObject.FindGameObjectWithTag("Healer");

        if (attacker != null)
        {
            attackerHealth = attacker.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogError("Attacker GameObject with tag 'Player' not found!");
        }

        if (healer != null)
        {
            healerHealth = healer.GetComponent<HealerHealth>();
        }
        else
        {
            Debug.LogError("Healer GameObject with tag 'Healer' not found!");
        }
    }

    void Update()
    {
        if (attackerHealth != null)
        {
            attackerHealthBarFill.fillAmount = (float)attackerHealth.currentHealth / attackerHealth.maxHealth;
        }

        if (healerHealth != null)
        {
            healerHealthBarFill.fillAmount = (float)healerHealth.currentHealth / healerHealth.maxHealth;
        }
    }
}
