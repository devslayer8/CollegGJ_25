using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;              // Array of waves to spawn
    public float timeBetweenWaves = 3f; // Time between waves
    public LayerMask wallLayerMask;   // Layer mask for walls (e.g. assign your 'Wall' layer here)
    public BoxCollider2D spawnArea;   // The area within which enemies can spawn
    public TMPro.TextMeshProUGUI waveStatusText; // UI Text for displaying wave status
    public LevelTransition levelTransition; // Reference to the level transition script

    private int currentWaveIndex = 0;
    private bool spawningWave = false;
    private bool playerInside = false;
    private bool healerInside = false;
    private int enemiesAlive = 0;

    private Transform attacker;
    private Transform healer;

    void Start()
    {
        if (spawnArea == null)
        {
            Debug.LogError("WaveSpawner is missing a BoxCollider2D component assigned to spawnArea!");
            return;
        }

        if (waveStatusText == null)
        {
            Debug.LogError("WaveSpawner is missing a reference to the waveStatusText UI Text!");
            return;
        }

        waveStatusText.text = "Waiting for player...";
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Healer"))
        {
            if (other.CompareTag("Player"))
            {
                playerInside = true;
                attacker = other.transform;
            }
            if (other.CompareTag("Healer"))
            {
                healerInside = true;
                healer = other.transform;
            }

            if (!spawningWave && currentWaveIndex < waves.Length)
            {
                StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.CompareTag("Healer"))
        {
            if (other.CompareTag("Player"))
            {
                playerInside = false;
                attacker = null;
            }
            if (other.CompareTag("Healer"))
            {
                healerInside = false;
                healer = null;
            }
        }
    }

    void Update()
    {
        // Check if all waves are completed
        if (AllWavesCompleted() && levelTransition != null)
        {
            levelTransition.CompleteWaves();
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        spawningWave = true;
        waveStatusText.text = $"Wave {currentWaveIndex + 1} started!";

        for (int i = 0; i < wave.enemyCount; i++)
        {
            SpawnEnemy(wave.enemyPrefab);
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        while (enemiesAlive > 0)
        {
            yield return null;
        }

        waveStatusText.text = $"Wave {currentWaveIndex + 1} ended!";
        currentWaveIndex++;
        spawningWave = false;

        if (currentWaveIndex < waves.Length)
        {
            for (float timer = timeBetweenWaves; timer > 0; timer -= Time.deltaTime)
            {
                waveStatusText.text = $"New wave incoming in {Mathf.Ceil(timer)} seconds";
                yield return null;
            }

            if ((playerInside || healerInside) && currentWaveIndex < waves.Length)
            {
                StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            }
        }
        else
        {
            waveStatusText.text = "All waves completed!";
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        int attempts = 0;
        bool validPositionFound = false;

        while (!validPositionFound && attempts < 10)
        {
            Vector2 randomPos = GetRandomPositionInSpawnArea();

            if (IsValidSpawnPosition(randomPos))
            {
                GameObject enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);
                validPositionFound = true;

                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    if (attacker != null)
                    {
                        enemyScript.SetAttacker(attacker);
                    }
                    if (healer != null)
                    {
                        enemyScript.SetHealer(healer);
                    }

                    enemiesAlive++;
                    enemyScript.OnEnemyKilled += OnEnemyKilled;
                }
            }

            attempts++;
        }
    }

    void OnEnemyKilled()
    {
        enemiesAlive--;
    }

    bool AllWavesCompleted()
    {
        return currentWaveIndex >= waves.Length && enemiesAlive == 0;
    }

    Vector2 GetRandomPositionInSpawnArea()
    {
        Bounds bounds = spawnArea.bounds;
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );
    }

    bool IsValidSpawnPosition(Vector2 position)
    {
        float checkRadius = 0.3f;  // Adjust as needed for your enemy's size
        Collider2D hit = Physics2D.OverlapCircle(position, checkRadius, wallLayerMask);
        return hit == null;
    }

    // Optional: Visualize the spawn area in the Scene view
    void OnDrawGizmos()
    {
        if (spawnArea != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(spawnArea.bounds.center, spawnArea.bounds.size);
        }
    }
}
