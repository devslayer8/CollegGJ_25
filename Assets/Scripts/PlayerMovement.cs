using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject gunPivot;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;

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
        controls.Attacker.Enable();
        controls.Attacker.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Attacker.Move.canceled += ctx => movement = Vector2.zero;
        controls.Attacker.Aim.performed += ctx => aimDirection = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        controls.Attacker.Shoot.performed += ctx => Shoot();
    }

    void OnDisable()
    {
        controls.Attacker.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        if (isGameOver) return; // Skip update if game is over

        RotateGun();
        UpdateAnimation();
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
        if (gunPivot == null || isGameOver) return;

        Vector3 direction = (aimDirection - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gunPivot.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Shoot()
    {
        if (isGameOver) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.linearVelocity = firePoint.right * bulletSpeed;
    }

    void UpdateAnimation()
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
