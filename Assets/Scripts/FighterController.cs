using UnityEngine;

public class FighterController : MonoBehaviour
{
    [Header("Fighter Identity")]
    public FighterData myFighterData; // Drag your Character Card here!

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    [Header("Combat Output")]
    public Transform projectileSpawnPoint; // Where do fireballs come out?

    public float currentMeter = 0f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float horizontalInput;
    private bool isBlocking = false;
    private bool isCrouching = false;

    [Header("Hitbox Setup")]
    public Transform attackPoint; // Where the punch happens
    public float attackRange = 0.5f; // How big the punch is
    public LayerMask enemyLayers; // What can we hit?

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Let's make sure the data loaded
        if (myFighterData != null)
        {
            Debug.Log("Fighter Loaded: " + myFighterData.characterName);
        }
    }

    void Update()
    {
        if (myFighterData == null) return; // Don't do anything if no character is selected!

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isBlocking = Input.GetKey(KeyCode.F);
        isCrouching = Input.GetKey(KeyCode.S) && isGrounded;

        horizontalInput = 0f;
        if (!isBlocking && !isCrouching)
        {
            if (Input.GetKey(KeyCode.D)) horizontalInput = 1f;
            if (Input.GetKey(KeyCode.A)) horizontalInput = -1f;
        }

        if (Input.GetKeyDown(KeyCode.W) && isGrounded && !isBlocking)
        {
            // We read the jump force from the Data Card!
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, myFighterData.jumpForce);
        }

        if (!isBlocking)
        {
            // Light and Heavy attacks are handled by the Animator later
            if (Input.GetKeyDown(KeyCode.H)) Debug.Log(myFighterData.characterName + " used Light Attack");
            if (Input.GetKeyDown(KeyCode.J)) Debug.Log(myFighterData.characterName + " used Heavy Attack");

            if (Input.GetKeyDown(KeyCode.K)) UseSpecial();
            if (Input.GetKeyDown(KeyCode.L)) AttemptUltimate();
        }
    }

    void FixedUpdate()
    {
        if (myFighterData != null)
        {
            // We read the move speed from the Data Card!
            rb.linearVelocity = new Vector2(horizontalInput * myFighterData.moveSpeed, rb.linearVelocity.y);
        }
    }

    void UseSpecial()
    {
        Debug.Log(myFighterData.characterName + " used Special!");

        // If this character has a projectile, spawn it!
        if (myFighterData.specialProjectilePrefab != null && projectileSpawnPoint != null)
        {
            Instantiate(myFighterData.specialProjectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        }
    }

    void PerformAttack(string attackType)
    {
        Debug.Log(myFighterData.characterName + " used: " + attackType);

        // 1. Determine damage based on attack type
        float damage = 0f;
        if (attackType == "Light") damage = 5f;
        if (attackType == "Heavy") damage = 15f;
        // (Special and Ultimate will be handled differently later)

        // 2. Draw an invisible circle at the attackPoint. Collect everything it hits on the Enemy layer.
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // 3. Loop through everything we hit and tell it to take damage
        foreach (Collider2D enemy in hitEnemies)
        {
            // Try to find the DummyController script on the thing we hit
            DummyController dummy = enemy.GetComponent<DummyController>();

            if (dummy != null)
            {
                dummy.TakeDamage(damage);
            }
        }
    }

    // This is a special Unity function that lets you SEE the invisible hitbox in the Editor window!
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    void AttemptUltimate()
    {
        if (currentMeter >= myFighterData.maxMeter)
        {
            Debug.Log(myFighterData.characterName + " used ULTIMATE!");
            currentMeter = 0f;

            if (myFighterData.ultimateEffectPrefab != null)
            {
                Instantiate(myFighterData.ultimateEffectPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}