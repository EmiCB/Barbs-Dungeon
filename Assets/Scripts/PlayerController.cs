using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    // Component references
    private Agent agent;

    // Movement variables
    private Vector2 movementInput = Vector2.zero;
    [SerializeField]
    private InputActionReference pointerPosition;
    private Vector2 pointerInput = Vector2.zero;

    public int dodgeCost = 1; // TODO: move this?

    // Combat references
    private WeaponParentController weaponParent;
    public bool isRollInProgress = false;

    // audio
    public float noiseEmissionRadius;

    // Initialize player
    void Start() {
        // Find unassigned components
        agent = GetComponent<Agent>();
        weaponParent = GetComponentInChildren<WeaponParentController>();
    }

    // Main game loop
    private void Update() {
        pointerInput = GetPointerInput();
        agent.agentMover.movementInput = movementInput;
        weaponParent.PointerPosition = pointerInput; // Weapon face cursor

        UpdateNoiseEmission();
        AnimateCharacter();
    }

    private void UpdateNoiseEmission() {
        // TODO: move these values to params
        // walking
        if (movementInput.magnitude > 0) {
            noiseEmissionRadius = 6.0f;
        } else {
            noiseEmissionRadius = 0.5f;
        }

        DetectNoiseReceivers();
    }

    /// <summary>
    /// Handle collider collisions.
    /// </summary>
    public void DetectNoiseReceivers() {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, noiseEmissionRadius)) {
            EnemyController enemyTarget = collider.GetComponent<EnemyController>();

            if (enemyTarget != null) {
                enemyTarget.SetIsChasing(true);
            }
        }
    }

    private IEnumerator DelayRoll() {
        yield return new WaitForSeconds( agent.statBlock.rollCooldown);
        isRollInProgress = false;
    }

    private void AnimateCharacter() {
        Vector2 lookDirection = pointerInput - (Vector2)transform.position;
        agent.agentAnimator.RotateToPointer(lookDirection);
        agent.agentAnimator.PlayWalkAnimation(movementInput);
    }

    public void ApplyDamage(int amount) {
        agent.healthSystem.RemoveAmount(amount);
    }

    // --- INPUT SYSTEM ---

    /// <summary>
    /// Gets and transforms the current cursor position from screen to world coordinates.
    /// </summary>
    /// <returns>The mouse position in world coodinates.</returns>
    private Vector2 GetPointerInput() {
        Vector3 mousePosition = pointerPosition.action.ReadValue<Vector2>();
        mousePosition.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

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
        agent.agentAnimator.PlayRollAnimation();
        isRollInProgress = true;
        StartCoroutine(DelayRoll());

        // Reduce stamina
        agent.staminaSystem.RemoveAmount(dodgeCost);
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
        agent.healthSystem.RemoveAmount(1);
    }

    /// <summary>
    /// Responds to Skill2 event from UnityInputSystem.
    /// </summary>
    void OnSkill2() {
        agent.healthSystem.AddAmount(1);
    }

    /// <summary>
    /// Responds to Skill3 event from UnityInputSystem.
    /// </summary>
    void OnSkill3() {
        agent.manaSystem.RemoveAmount(1);
    }

    /// <summary>
    /// Responds to Skill4 event from UnityInputSystem.
    /// </summary>
    void OnSkill4() {
        agent.manaSystem.AddAmount(1);
    }

    // --- DEBUG ---

    /// <summary>
    /// Draw gizmos to help visualize in the editor.
    /// </summary>
    private void OnDrawGizmosSelected() {
        // Display raycats attack circle
        Gizmos.color = Color.cyan;
        Vector3 position = transform.position == null ? Vector3.zero : transform.position;
        Gizmos.DrawWireSphere(position, noiseEmissionRadius);
    }
}
