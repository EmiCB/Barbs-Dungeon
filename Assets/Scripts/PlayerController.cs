using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    // Component references
    private AgentMover agentMover;
    private AgentAnimator agentAnimator;

    // Entity data
    public StatBlock statBlock;

    // Movement variables
    private Vector2 movementInput = Vector2.zero;
    [SerializeField]
    private InputActionReference pointerPosition;
    private Vector2 pointerInput = Vector2.zero;

    // Health system
    private ResourceSystem healthSystem;
    public ResourceBar healthBar;

    // Mana system
    private ResourceSystem manaSystem;
    public ResourceBar manaBar;

    // Stamina system
    private ResourceSystem staminaSystem;
    public ResourceBar staminaBar;
    public int dodgeCost = 1; // TOOD: move this?

    // Combat references
    private WeaponParentController weaponParent;
    private bool isRollInProgress = false;

    // Initialize player
    void Start() {
        // Find unassigned components
        agentMover = GetComponent<AgentMover>();
        agentAnimator = GetComponentInChildren<AgentAnimator>();
        weaponParent = GetComponentInChildren<WeaponParentController>();

        // Initialize stats
        healthSystem = new ResourceSystem(statBlock.baseHealth, healthBar);
        manaSystem = new ResourceSystem(statBlock.baseMana, manaBar);
        staminaSystem = new ResourceSystem(statBlock.baseStamina, staminaBar);
    }

    // Main game loop
    private void Update() {
        pointerInput = GetPointerInput();
        agentMover.movementInput = movementInput;
        weaponParent.PointerPosition = pointerInput; // Weapon face cursor
        AnimateCharacter();
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

    private IEnumerator DelayRoll() {
        yield return new WaitForSeconds(statBlock.rollCooldown);
        isRollInProgress = false;
    }

    private void AnimateCharacter() {
        Vector2 lookDirection = pointerInput - (Vector2)transform.position;
        agentAnimator.RotateToPointer(lookDirection);
        agentAnimator.PlayWalkAnimation(movementInput);
    }

    // --- INPUT SYSTEM ---

    /// <summary>
    /// Responds to Move event from UnityInputSystem.
    /// </summary>
    /// <param name="value">The action value that contains the Vector2 X/Y input from the player's input device.</param>
    void OnMove(InputValue value) {
        movementInput = value.Get<Vector2>().normalized;
    }

    void OnRoll() {
        if (isRollInProgress) { return; }

        // Trigger roll animation
        agentAnimator.PlayRollAnimation();
        isRollInProgress = true;
        StartCoroutine(DelayRoll());

        // Reduce stamina
        staminaSystem.RemoveAmount(dodgeCost);
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
