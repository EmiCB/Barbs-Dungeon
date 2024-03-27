using UnityEngine;

public class Agent : MonoBehaviour {
    // Component references
    public AgentMover agentMover;
    public AgentAnimator agentAnimator;

    // Agent data
    public StatBlock statBlock;

    // Resource systems
    public ResourceSystem healthSystem;
    public ResourceSystem manaSystem;
    public ResourceSystem staminaSystem;

    // Optional resource display bars
    public ResourceBar healthBar;
    public ResourceBar manaBar;
    public ResourceBar staminaBar;

    // Weapon
    private WeaponParentController weaponParent;

    // Movement
    public Vector2 movementDirection;
    public Vector2 aimDirection;

    // TODO: change back to Awake and fix script execution error w/ ResourceBar
    void Start() { 
        // Find unassigned components
        agentMover = GetComponent<AgentMover>();
        agentAnimator = GetComponentInChildren<AgentAnimator>();
        weaponParent = GetComponentInChildren<WeaponParentController>();

        // Create resource systems
        healthSystem = new ResourceSystem(statBlock.baseHealth, healthBar);
        manaSystem = new ResourceSystem(statBlock.baseMana, manaBar);
        staminaSystem = new ResourceSystem(statBlock.baseStamina, staminaBar);
    }

    void Update() {
        agentMover.movementInput = movementDirection;
        weaponParent.PointerPosition = aimDirection; // Weapon face cursor
    }
}
