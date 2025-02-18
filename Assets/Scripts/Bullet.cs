using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage = 3;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet collided with: " + collision.gameObject.name); // Log collision

        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy != null)
            {
                Debug.Log("Enemy detected! Applying damage: " + bulletDamage);
                enemy.TakeDamage(bulletDamage);
            }
            else
            {
                Debug.LogError("Enemy script not found on the object!");
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            Debug.Log("Bullet hit a wall. Destroying.");
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Healer")) // Ignore collisions with healer
        {
            Debug.Log("Bullet ignored the healer.");
        }
    }
}
