using UnityEngine;
using UnityEngine.InputSystem;

public class HealerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject gunPivot;
    public Transform firePoint;
    public GameObject healBulletPrefab;
    public float bulletSpeed = 10f;
    public int maxHealBullets = 20;  // Total bullet capacity
    private int currentHealBullets; // Tracks available bullets

    private PlayerControls controls;
    private Vector2 movement;
    private Vector2 aimDirection;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGameOver = false; // Add this variable

    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Healer.Enable();
        controls.Healer.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Healer.Move.canceled += ctx => movement = Vector2.zero;
        controls.Healer.Aim.performed += ctx => aimDirection = ctx.ReadValue<Vector2>();
        controls.Healer.Shoot.performed += ctx => ShootHealBullet();
    }

    void OnDisable()
    {
        controls.Healer.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealBullets = maxHealBullets; // Start with max bullets
    }

    void Update()
    {
        if (isGameOver) return; // Skip update if game is over

        RotateGun();
        AnimateMovement();
    }

    void FixedUpdate()
    {
        if (isGameOver)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = movement * moveSpeed;
    }

    void RotateGun()
    {
        if (gunPivot == null || aimDirection.magnitude < 0.1f || isGameOver) return;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        gunPivot.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void ShootHealBullet()
    {
        if (isGameOver) return;

        if (currentHealBullets > 0)
        {
            currentHealBullets--; // Reduce available bullets before shooting

            GameObject healBullet = Instantiate(healBulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D bulletRb = healBullet.GetComponent<Rigidbody2D>();
            bulletRb.linearVelocity = firePoint.right * bulletSpeed;

            Debug.Log("Healer shot a bullet. Remaining: " + currentHealBullets);
        }
        else
        {
            Debug.Log("No heal bullets left!");
        }
    }

    public void AddHealBullets(int amount)
    {
        int bulletsToAdd = Mathf.Min(amount, maxHealBullets - currentHealBullets);
        currentHealBullets += bulletsToAdd;
        Debug.Log("Picked up heal bullets. Current: " + currentHealBullets);
    }

    public int GetCurrentAmmo()
    {
        return currentHealBullets;
    }

    public bool CanPickupAmmo()
    {
        return currentHealBullets < maxHealBullets;
    }

    void AnimateMovement()
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", movement.magnitude);
        }
    }

    public void SetGameOver(bool state)
    {
        isGameOver = state; // Set the game over state
    }
}
