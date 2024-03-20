using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO: split out into general agent class (generalizes to enemies) + player input class
public class PlayerController : MonoBehaviour {
    public StatBlock statBlock;

    // Set up animnation
    private Animator animator;

    // Movement variables
    private float currentMoveSpeed = 0.0f;
    private Vector2 movementInput = Vector2.zero;
    private Vector2 movementInputOld = Vector2.zero;
    [SerializeField]
    private InputActionReference pointerPosition;
    private Vector2 pointerInput = Vector2.zero;
    private bool facingRight = true;

    // Health system
    private ResourceSystem healthSystem;
    public ResourceBar healthBar;

    // Mana system
    private ResourceSystem manaSystem;
    public ResourceBar manaBar;

    // Component references
    private Rigidbody2D rb;

    // Combat references
    private WeaponParentController weaponParent;
    public bool IsRolling { get; private set; }
    private bool isRollInProgress = false;

    // Initialize player
    void Start() {
        // Find unassigned components
        rb = GetComponent<Rigidbody2D>();
        weaponParent = GetComponentInChildren<WeaponParentController>();
        animator = GetComponent<Animator>();

        // Initialize stats
        healthSystem = new ResourceSystem(statBlock.baseHealth, healthBar);
        manaSystem = new ResourceSystem(statBlock.baseMana, manaBar);
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
            currentMoveSpeed += statBlock.acceleration * statBlock.baseMoveSpeed * Time.deltaTime;
        } else {
            currentMoveSpeed -= statBlock.deceleration * statBlock.baseMoveSpeed * Time.deltaTime;
        }
        currentMoveSpeed = Mathf.Clamp(currentMoveSpeed, 0, statBlock.baseMoveSpeed);
        rb.velocity = movementInputOld * currentMoveSpeed;

        // Integrate with animator
        if (rb.velocity != Vector2.zero) {
            animator.SetBool("isWalking", true);
        } else {
            animator.SetBool("isWalking", false);
        }
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

    public void ResetIsRolling() {
        IsRolling = false;
    }

    private IEnumerator DelayRoll() {
        yield return new WaitForSeconds(statBlock.rollCooldown);
        isRollInProgress = false;
    }

    // --- INPUT SYSTEM ---

    /// <summary>
    /// Responds to Move event from UnityInputSystem.
    /// </summary>
    /// <param name="value">The action value that contains the Vector2 X/Y input from the player's input device.</param>
    void OnMove(InputValue value) {
        movementInput = value.Get<Vector2>();
    }

    void OnRoll() {
        if (isRollInProgress) { return; }

        // Trigger roll animation
        animator.SetTrigger("Roll");
        IsRolling = true;
        isRollInProgress = true;
        StartCoroutine(DelayRoll());
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
}
