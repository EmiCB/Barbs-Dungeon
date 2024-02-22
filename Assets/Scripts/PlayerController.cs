using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    // Movement variables
    private float moveSpeed = 10.0f;
    private Vector2 movementInput = Vector2.zero;

    // Health system
    private int maxHealth = 10;     // TODO: split out into a character stats ScriptableObject
    private ResourceSystem healthSystem;
    public ResourceBar healthBar;

    // Mana system
    private int maxMana = 10;       // TODO: split out into a character stats ScriptableObject
    private ResourceSystem manaSystem;
    public ResourceBar manaBar;

    // Component references
    private Rigidbody2D rb;

    // Initialize player
    void Start() {
        rb = GetComponent<Rigidbody2D>();

        // initialize stats
        healthSystem = new ResourceSystem(maxHealth, healthBar);
        manaSystem = new ResourceSystem(maxMana, manaBar);
    }

    // Run physics
    private void FixedUpdate() {
        rb.velocity = movementInput * moveSpeed;
    }

    // --- INPUT SYSTEM ---

    /// <summary>
    /// Responds to OnMove event from UnityInputSystem.
    /// </summary>
    /// <param name="value">The action value that contains the Vector2 X/Y input from the player's input device.</param>
    void OnMove(InputValue value) {
        movementInput = value.Get<Vector2>();
    }

    // Using skill buttons as temp tests for health system
    void OnSkill1() {
        healthSystem.RemoveAmount(1);
    }

    void OnSkill2() {
        healthSystem.AddAmount(1);
    }

    void OnSkill3() {
        manaSystem.RemoveAmount(1);
    }

    void OnSkill4() {
        manaSystem.AddAmount(1);
    }
}
