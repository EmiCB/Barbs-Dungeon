using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    // Component references
    private Agent agent;

    [SerializeField]
    private ClassAbilityGeneric abilities;

    // Movement variables
    private Vector2 movementInput = Vector2.zero;
    [SerializeField]
    private InputActionReference pointerPosition;
    private Vector2 pointerInput = Vector2.zero;

    // Combat references
    private WeaponParentController weaponParent;
    public bool IsRollOnCooldown = false;
    public int rollFrameCounter = 0;

    // audio
    public float idleNoiseRadius;   // 0.5f
    public float walkNoiseRadius;   // 6.0f
    public float sneakNoiseRadius;
    public float sprintNoiseRadius;

    private float noiseEmissionRadius;

    // ui yikes lol
    public GameObject deathScreen;

    // sound fx
    [SerializeField] private AudioClip takeDamageClip;

    // Initialize player
    void Start() {
        // Find unassigned components
        agent = GetComponent<Agent>();
        weaponParent = GetComponentInChildren<WeaponParentController>();

        abilities = new BeserkerAbilities(this);
    }

    // Main game loop
    private void Update() {
        pointerInput = GetPointerInput();
        if (rollFrameCounter == 0) { agent.movementDirection = movementInput; } // Don't update if agent is in roll frames

        weaponParent.PointerPosition = pointerInput; // Weapon face cursor

        UpdateNoiseEmission();
        AnimateCharacter();
    }

    private void FixedUpdate()
    {
        rollFrameCounter = Mathf.Min(rollFrameCounter + 1, 0);
    }

    private void UpdateNoiseEmission() {
        // walking
        if (movementInput.magnitude > 0) {
            noiseEmissionRadius = walkNoiseRadius;
        } 

        // idle
        else {
            noiseEmissionRadius = idleNoiseRadius;
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
        IsRollOnCooldown = false;
    }

    private void AnimateCharacter() {
        Vector2 lookDirection = pointerInput - (Vector2)transform.position;
        agent.agentAnimator.RotateToPointer(lookDirection);
        agent.agentAnimator.PlayWalkAnimation(movementInput);
    }

    // damage + heal
    public void ApplyDamage(int amount) {
        agent.healthSystem.RemoveAmount(amount);

        SoundFXManager.instance.PlaySoundFXClip(takeDamageClip, transform, 1.0f);

        // check if player dead
        if (agent.healthSystem.GetCurrentValue() <= 0) {
            OnPlayerDeath();
        }
    }
    public void Heal(int amount) {
        agent.healthSystem.AddAmount(amount);
    }

    public void DrainMana(int amount) {
        agent.manaSystem.RemoveAmount(amount);
    }
    public void RecoverMana(int amount) {
        agent.manaSystem.AddAmount(amount);
    }

    public void DrainStamina(int amount) {
        agent.staminaSystem.RemoveAmount(amount);
    }
    public void RecoverStamina(int amount) {
        agent.staminaSystem.AddAmount(amount);
    }

    public void OnPlayerDeath() {
        // open death menu
        deathScreen.SetActive(true);
        // set timescale to 0
        Time.timeScale = 0.0f;
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
        if (IsRollOnCooldown) { return; }

        // Trigger roll animation
        agent.agentAnimator.PlayRollAnimation();
        IsRollOnCooldown = true;
        StartCoroutine(DelayRoll());

        // Reduce stamina
        agent.staminaSystem.RemoveAmount(agent.statBlock.rollCost);

        rollFrameCounter -= agent.statBlock.rollFrames;
    }

    /// <summary>
    /// Responds to Attack event from UnityInputSystem.
    /// </summary>
    void OnAttack() {
        if (rollFrameCounter != 0) { return; }

        weaponParent.Attack();
    }

    // Using skill buttons as temp tests for health system

    /// <summary>
    /// Responds to Skill1 event from UnityInputSystem.
    /// </summary>
    void OnSkill1() {
        if (rollFrameCounter != 0) { return; }

        abilities.OnSkill1();
    }

    /// <summary>
    /// Responds to Skill2 event from UnityInputSystem.
    /// </summary>
    void OnSkill2() {
        if (rollFrameCounter != 0) { return; }

        abilities.OnSkill2();
    }

    /// <summary>
    /// Responds to Skill3 event from UnityInputSystem.
    /// </summary>
    void OnSkill3() {
        if (rollFrameCounter != 0) { return; }

        abilities.OnSkill3();
    }

    /// <summary>
    /// Responds to Skill4 event from UnityInputSystem.
    /// </summary>
    void OnSkill4() {
        if (rollFrameCounter != 0) { return; }

        abilities.OnSkill4();
    }

    public void AddBuff(Func<StatBlock, StatBlock> buff, int seconds)
    {
        agent.AddBuff(buff, seconds);
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
