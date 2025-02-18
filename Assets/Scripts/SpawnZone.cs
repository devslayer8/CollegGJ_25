using UnityEngine;
using System.Collections.Generic;

public class SpawnZone : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject enemyPrefab;    // Enemy prefab (assign in Inspector)
    public int maxEnemies = 5;        // Maximum number of enemies to spawn

    [Header("Collision Settings")]
    [Tooltip("Layer(s) that represent walls or obstacles. Enemies will not spawn where these are detected.")]
    public LayerMask wallLayerMask;   // Layer mask for walls (e.g. assign your 'Wall' layer here)

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool playerInside = false;
    private BoxCollider2D spawnZoneCollider;

    void Start()
    {
        // Get the BoxCollider2D attached to this SpawnZone (ensure it's set as a Trigger)
        spawnZoneCollider = GetComponent<BoxCollider2D>();
        if (spawnZoneCollider == null)
        {
            Debug.LogError("SpawnZone is missing a BoxCollider2D component!");
            return;
        }

        SpawnEnemies();
    }

    void Update()
    {
        if (playerInside)
        {
            // Debug message removed to avoid irritation
        }
    }

    /// <summary>
    /// Spawns enemies randomly inside the collider bounds, ensuring they do not overlap walls.
    /// </summary>
    void SpawnEnemies()
    {
        int attempts = 0;
        // Use the collider's bounds to define the spawn area.
        Bounds bounds = spawnZoneCollider.bounds;

        // Loop until we've spawned the desired number or we reach our attempt limit.
        while (spawnedEnemies.Count < maxEnemies && attempts < maxEnemies * 10)
        {
            // Pick a random position within the collider bounds.
            Vector2 randomPos = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );

            // Check that this position doesn't overlap with a wall.
            if (IsValidSpawnPosition(randomPos))
            {
                GameObject enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.SetSpawnZone(this);
                }
                spawnedEnemies.Add(enemy);
            }
            attempts++;
        }

        Debug.Log("Spawned " + spawnedEnemies.Count + " enemies (attempts: " + attempts + ")");
    }

    /// <summary>
    /// Returns true if the position is free of walls (using the provided layer mask).
    /// Adjust the radius to match the approximate enemy size.
    /// </summary>
    bool IsValidSpawnPosition(Vector2 position)
    {
        float checkRadius = 0.3f;  // Adjust as needed for your enemy's size
        Collider2D hit = Physics2D.OverlapCircle(position, checkRadius, wallLayerMask);
        return hit == null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            // Debug message removed to avoid irritation

            // Notify all spawned enemies that the player is in range.
            foreach (GameObject enemy in spawnedEnemies)
            {
                if (enemy != null)
                {
                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    if (enemyScript != null)
                    {
                        enemyScript.SetAttacker(other.transform); // Set attacker for enemies
                    }
                }
            }
        }
        else if (other.CompareTag("Healer"))
        {
            playerInside = true;
            // Debug message removed to avoid irritation

            // Notify all spawned enemies that the healer is in range.
            foreach (GameObject enemy in spawnedEnemies)
            {
                if (enemy != null)
                {
                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    if (enemyScript != null)
                    {
                        enemyScript.SetHealer(other.transform); // Set healer for enemies
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            // Debug message removed to avoid irritation

            // Notify all spawned enemies that the player is no longer in range.
            foreach (GameObject enemy in spawnedEnemies)
            {
                if (enemy != null)
                {
                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    if (enemyScript != null)
                    {
                        enemyScript.SetAttacker(null); // Clear attacker for enemies
                    }
                }
            }
        }
        else if (other.CompareTag("Healer"))
        {
            playerInside = false;
            // Debug message removed to avoid irritation

            // Notify all spawned enemies that the healer is no longer in range.
            foreach (GameObject enemy in spawnedEnemies)
            {
                if (enemy != null)
                {
                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    if (enemyScript != null)
                    {
                        enemyScript.SetHealer(null); // Clear healer for enemies
                    }
                }
            }
        }
    }

    // Optional: Visualize the spawn area in the Scene view
    void OnDrawGizmos()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
        }
    }
}
