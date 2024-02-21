using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    // Movement variables
    private float moveSpeed = 10.0f;
    private Vector2 movementInput = Vector2.zero;

    // Health system
    private int maxHealth = 10; // TODO: split out into a character stats ScriptableObject
    private HealthSystem healthSystem;
    public HealthBar healthBar;

    // Component references
    private Rigidbody2D rb;

    // Initialize player
    void Start() {
        rb = GetComponent<Rigidbody2D>();

        // initialize stats
        healthSystem = new HealthSystem(maxHealth, healthBar);
    }

    // Run physics
    private void FixedUpdate() {
        rb.velocity = movementInput * moveSpeed;
    }

    /// <summary>
    /// Responds to OnMove event from UnityInputSystem.
    /// </summary>
    /// <param name="value">The action value that contains the Vector2 X/Y input from the player's input device.</param>
    void OnMove(InputValue value) {
        movementInput = value.Get<Vector2>();
    }

    // Using skill buttons as temp tests for health system
    void OnSkill1() {
        healthSystem.ApplyDamage(1);
    }

    void OnSkill2() {
        healthSystem.ApplyHeal(1);
    }
}
