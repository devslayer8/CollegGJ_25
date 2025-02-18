using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int health = 10;
    public int damage = 10;
    public int minHealthLimit = -50; // Cannot go below this HP
    public float obstacleAvoidanceDistance = 1.5f;
    public float separationDistance = 1f; // Minimum distance to keep from other enemies

    public delegate void EnemyKilled();
    public event EnemyKilled OnEnemyKilled;

    private Transform attacker;  // The shooter
    private Transform healer;    // The healer
    private SpawnZone spawnZone;
    private bool attackerInsideZone = false;
    private bool healerInsideZone = false;
    private EnemyHealthUI healthUI;
    private SpriteRenderer spriteRenderer;
    private LayerMask wallLayer;
    private Vector2 currentDirection;

    void Awake()
    {
        healthUI = GetComponentInChildren<EnemyHealthUI>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (healthUI == null)
            Debug.LogError("EnemyHealthUI component not found on " + gameObject.name);
        if (spriteRenderer == null)
            Debug.LogError("SpriteRenderer component not found on " + gameObject.name);

        wallLayer = LayerMask.GetMask("Wall");
    }

    void Start()
    {
        if (healthUI != null)
            healthUI.UpdateHealth(health);

        currentDirection = Vector2.zero;
    }

    public void SetSpawnZone(SpawnZone zone)
    {
        spawnZone = zone;
    }

    public void SetAttacker(Transform target)
    {
        attacker = target;
        attackerInsideZone = (attacker != null);
    }

    public void SetHealer(Transform target)
    {
        healer = target;
        healerInsideZone = (healer != null);
    }

    /// <summary>
    /// Heals the enemy. If health reaches 0, the enemy is destroyed.
    /// </summary>
    public void Heal(int amount)
    {
        if (health >= 0)
        {
            Debug.Log("Enemy is above 0! Cannot heal further.");
            return;
        }

        health += amount;

        // Ensure the healer cannot heal past 0
        if (health >= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy fully healed and removed!");

            if (OnEnemyKilled != null)
            {
                OnEnemyKilled();
            }

            return;
        }

        Debug.Log("Enemy healed: +" + amount + " HP | Current Health: " + health);

        if (healthUI != null)
        {
            healthUI.UpdateHealth(health);
        }
    }

    public int GetHealth()
    {
        return health;
    }

    void Update()
    {
        if ((attackerInsideZone && attacker != null) || (healerInsideZone && healer != null))
        {
            Transform target = null;

            if (attackerInsideZone && healerInsideZone)
            {
                // Choose the closer target between attacker and healer
                float distanceToAttacker = attacker != null ? Vector2.Distance(transform.position, attacker.position) : float.MaxValue;
                float distanceToHealer = healer != null ? Vector2.Distance(transform.position, healer.position) : float.MaxValue;

                target = (distanceToAttacker < distanceToHealer) ? attacker : healer;
            }
            else if (attackerInsideZone)
            {
                target = attacker;
            }
            else if (healerInsideZone)
            {
                target = healer;
            }

            if (target != null)
            {
                Vector2 directionToTarget = (target.position - transform.position).normalized;
                directionToTarget = AvoidOtherEnemies(directionToTarget);

                if (!IsPathBlocked(directionToTarget))
                {
                    currentDirection = directionToTarget;
                }
                else
                {
                    currentDirection = FindAlternativePath(directionToTarget);
                }

                transform.position += (Vector3)(currentDirection * speed * Time.deltaTime);
            }
        }
    }

    Vector2 AvoidOtherEnemies(Vector2 direction)
    {
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, separationDistance);
        Vector2 separationForce = Vector2.zero;

        foreach (Collider2D collider in nearbyEnemies)
        {
            if (collider.gameObject != gameObject && collider.CompareTag("Enemy"))
            {
                Vector2 toOther = transform.position - collider.transform.position;
                separationForce += toOther.normalized / toOther.magnitude;
            }
        }

        return (direction + separationForce).normalized;
    }

    bool IsPathBlocked(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, obstacleAvoidanceDistance, wallLayer);
        return hit.collider != null;
    }

    Vector2 FindAlternativePath(Vector2 originalDirection)
    {
        Vector2 leftDirection = Vector2.Perpendicular(originalDirection);
        Vector2 rightDirection = -leftDirection;

        if (!IsPathBlocked(leftDirection))
            return leftDirection;
        else if (!IsPathBlocked(rightDirection))
            return rightDirection;

        return -originalDirection;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                int attackDamage = (health > 0) ? 10 : 20;
                playerHealth.TakeDamage(attackDamage);
                Debug.Log("Enemy attacked player! Damage: -" + attackDamage);
            }
        }
        else if (collision.gameObject.CompareTag("Healer"))
        {
            HealerHealth healerHealth = collision.gameObject.GetComponent<HealerHealth>();
            if (healerHealth != null)
            {
                int attackDamage = (health > 0) ? 10 : 20;
                healerHealth.TakeDamage(attackDamage);
                Debug.Log("Enemy attacked healer! Damage: -" + attackDamage);
            }
        }
    }

    /// <summary>
    /// Handles taking damage and updating health, but doesn't go below the min limit.
    /// </summary>
    public void TakeDamage(int amount)
    {
        if (health <= minHealthLimit)
        {
            Debug.Log("Enemy reached minimum health! Cannot take more damage.");
            return;
        }

        health -= amount;

        // Ensure health does not drop below minHealthLimit
        if (health < minHealthLimit)
        {
            health = minHealthLimit;
        }

        Debug.Log("Enemy took damage: -" + amount + " HP | Current Health: " + health);

        if (healthUI != null)
        {
            healthUI.UpdateHealth(health);
        }

        if (health < 0 && spriteRenderer != null)
        {
            spriteRenderer.color = Color.magenta;
        }

        if (health <= 0)
        {
            if (health <= minHealthLimit)
            {
                Destroy(gameObject);
                if (OnEnemyKilled != null)
                {
                    OnEnemyKilled();
                }
            }
        }
    }
}
