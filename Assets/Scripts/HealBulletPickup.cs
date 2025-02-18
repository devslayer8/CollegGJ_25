using UnityEngine;

public class HealBulletPickup : MonoBehaviour
{
    public int ammoAmount = 10; // Amount of bullets granted per pickup

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Healer"))  // Only the healer can pick it up
        {
            HealerMovement healer = other.GetComponent<HealerMovement>();
            if (healer != null)
            {
                if (healer.CanPickupAmmo())  // Check if there's space for more bullets
                {
                    healer.AddHealBullets(ammoAmount);
                    Debug.Log("Healer picked up heal bullets. Current ammo: " + healer.GetCurrentAmmo());
                    Destroy(gameObject); // Remove the pickup after use
                }
                else
                {
                    Debug.Log("Healer already has max ammo. Can't pick up more!");
                }
            }
        }
    }
}
