using UnityEngine;
using UnityEngine.InputSystem;

// TODO: split out into general agent class (generalizes to enemies) + player input class
public class PlayerController : MonoBehaviour {
    // Movement variables
    private float maxMoveSpeed = 10.0f;     // TODO: split out into a character stats ScriptableObject
    private float currentMoveSpeed = 0.0f;
    private float acceleration = 5.0f;      // TODO: split out into a character stats ScriptableObject
    private float deceleration = 5.0f;      // TODO: split out into a character stats ScriptableObject
    private Vector2 movementInput = Vector2.zero;
    private Vector2 movementInputOld = Vector2.zero;
    [SerializeField]
    private InputActionReference pointerPosition;
    private Vector2 pointerInput = Vector2.zero;
    private bool facingRight = true;

    // Health system
    private int maxHealth = 10;     // TODO: split out into a character stats ScriptableObject
    private ResourceSystem healthSystem;
    public ResourceBar healthBar;

    // Mana system
    private int maxMana = 10;       // TODO: split out into a character stats ScriptableObject
    private ResourceSystem manaSystem;
    public ResourceBar manaBar;

    // Combat stats
    private int baseDamage = 1;      // TODO: split out into a character stats ScriptableObject

    // Component references
    private Rigidbody2D rb;

    // Combat references
    private WeaponParentController weaponParent;

    // Initialize player
    void Start() {
        // Find unassigned components
        rb = GetComponent<Rigidbody2D>();
        weaponParent = GetComponentInChildren<WeaponParentController>();

        // Initialize stats
        healthSystem = new ResourceSystem(maxHealth, healthBar);
        manaSystem = new ResourceSystem(maxMana, manaBar);
    }

    // Main game loop
    private void Update() {
        pointerInput = GetPointerInput();

        // Player face mouse cursor
        if (pointerInput.x < transform.position.x && facingRight) {
            facingRight = !facingRight;
            transform.Rotate(0f, 180f, 0f);
        }
        else if (pointerInput.x > transform.position.x && !facingRight) {
            facingRight = !facingRight;
            transform.Rotate(0f, 180f, 0f);
        }

        // Weapon face cursor
        weaponParent.PointerPosition = pointerInput;
    }

    // Run physics
    private void FixedUpdate() {
        // Make smoother movement
        if (movementInput.magnitude > 0 && currentMoveSpeed >= 0) {
            movementInputOld = movementInput;
            currentMoveSpeed += acceleration * maxMoveSpeed * Time.deltaTime;
        } else {
            currentMoveSpeed -= deceleration * maxMoveSpeed * Time.deltaTime;
        }
        currentMoveSpeed = Mathf.Clamp(currentMoveSpeed, 0, maxMoveSpeed);
        rb.velocity = movementInputOld * currentMoveSpeed;
    }

    /// <summary>
    /// Gets and transforms the current cursor position from screen to world coordinates.
    /// </summary>
    /// <returns>The mouse position in world coodinates.</returns>
    private Vector2 GetPointerInput() {
        Vector3 mousePosition = pointerPosition.action.ReadValue<Vector2>();
        mousePosition.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    // --- INPUT SYSTEM ---

    /// <summary>
    /// Responds to Move event from UnityInputSystem.
    /// </summary>
    /// <param name="value">The action value that contains the Vector2 X/Y input from the player's input device.</param>
    void OnMove(InputValue value) {
        movementInput = value.Get<Vector2>();
    }

    /// <summary>
    /// Responds to Attack event from UnityInputSystem.
    /// </summary>
    void OnAttack() {
        weaponParent.Attack();
    }

    // Using skill buttons as temp tests for health system

    /// <summary>
    /// Responds to Skill1 event from UnityInputSystem.
    /// </summary>
    void OnSkill1() {
        healthSystem.RemoveAmount(1);
    }

    /// <summary>
    /// Responds to Skill2 event from UnityInputSystem.
    /// </summary>
    void OnSkill2() {
        healthSystem.AddAmount(1);
    }

    /// <summary>
    /// Responds to Skill3 event from UnityInputSystem.
    /// </summary>
    void OnSkill3() {
        manaSystem.RemoveAmount(1);
    }

    /// <summary>
    /// Responds to Skill4 event from UnityInputSystem.
    /// </summary>
    void OnSkill4() {
        manaSystem.AddAmount(1);
    }

    // --- GETTERS + SETTERS ---

    /// <returns>Get the player's current base damage.</returns>
    public int getBaseDamage() {
        return baseDamage;
    }
}
