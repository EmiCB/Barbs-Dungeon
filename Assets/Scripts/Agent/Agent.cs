using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Agent : MonoBehaviour {
    // Component references
    public AgentMover agentMover;
    public AgentAnimator agentAnimator;

    // Agent data
    public StatBlock ogStatBlock;

    [NonSerialized]
    public StatBlock statBlock;

    // Buffs
    public Dictionary<GUID, Func<StatBlock, StatBlock>> buffs = new Dictionary<GUID, Func<StatBlock, StatBlock>>();

    // Resource systems
    public ResourceSystem healthSystem;
    public ResourceSystem manaSystem;
    public ResourceSystem staminaSystem;

    // Resource regeneration timers
    private float healthRegenerationTime;
    private float healthRegenerationTimer;
    private float manaRegenerationTime;
    private float manaRegenerationTimer;
    private float staminaRegenerationTime;
    private float staminaRegenerationTimer;

    // Optional resource display bars
    public ResourceBar healthBar;
    public ResourceBar manaBar;
    public ResourceBar staminaBar;

    // Weapon
    private WeaponParentController weaponParent;

    // Movement
    public Vector2 movementDirection;
    public Vector2 aimDirection;

    private void Awake()
    {
        statBlock = (StatBlock) ogStatBlock.Clone();
    }

    void Start() { 
        // Find unassigned components
        agentMover = GetComponent<AgentMover>();
        agentAnimator = GetComponentInChildren<AgentAnimator>();
        weaponParent = GetComponentInChildren<WeaponParentController>();

        // Create resource systems
        healthSystem = new ResourceSystem(statBlock.baseHealth, healthBar);
        manaSystem = new ResourceSystem(statBlock.baseMana, manaBar);
        staminaSystem = new ResourceSystem(statBlock.baseStamina, staminaBar);

        // Initialize timers
        UpdateResourceRegenerationTimes();

        // Initalize stats
        calculateStatBlock();
    }

    void Update() {
        agentMover.movementInput = movementDirection;
        weaponParent.PointerPosition = aimDirection; // Weapon face cursor

        // Regenerate resources over time
        RegenerateResource(ref healthRegenerationTimer, healthRegenerationTime, healthSystem);
        RegenerateResource(ref manaRegenerationTimer, manaRegenerationTime, manaSystem);
        RegenerateResource(ref staminaRegenerationTimer, staminaRegenerationTime, staminaSystem);
    }

    private void UpdateResourceRegenerationTimes() {
        // Health
        healthRegenerationTime = 100.0f;
        healthRegenerationTimer = 0.0f;

        // Mana
        manaRegenerationTime = 100 - (statBlock.wisdom * 10);
        manaRegenerationTimer = 0.0f;

        // Stamina
        staminaRegenerationTime = 100 - (statBlock.agility * 10);
        staminaRegenerationTimer = 0.0f;
    }

    private void RegenerateResource(ref float timer, float time, ResourceSystem system) {
        timer += Time.deltaTime;

        if (timer >= time && system.GetCurrentValue() < system.GetMaxValue()) {
            system.AddAmount(1);
            timer = 0.0f;
        }
    }

    // Could maybe refractor of agent buff later...
    public void AddBuff(Func<StatBlock, StatBlock> buff, int seconds) {
        GUID guid = GUID.Generate();
        buffs.Add(guid, buff);

        Debug.Log("Added Buff");

        StartCoroutine(DelayBuff(guid, seconds));
        calculateStatBlock();
    }
    private IEnumerator DelayBuff(GUID guid, int seconds)
    {
        yield return new WaitForSeconds(seconds);
        buffs.Remove(guid);

        Debug.Log("Removed Buff");
        calculateStatBlock();
    }

    private void calculateStatBlock()
    {
        statBlock = (StatBlock) ogStatBlock.Clone();
        foreach (var i in buffs.Values)
        {
            statBlock = i(statBlock);
        }
    }

    // --- FUNCTIONS --

    /// <summary>
    /// Apply damage to this enemy.
    /// </summary>
    /// <param name="amount"></param>
    public void ApplyDamage(int amount)
    {
        healthSystem.RemoveAmount(amount);

        // Check if enemy is dead and remove it from the scene.
        if (healthSystem.GetCurrentValue() <= 0)
        {
            // TODO: make ObjectPooler for enemies to help increase performance + reduce possible
            // memory leaks
            gameObject.SetActive(false);
        }
    }
}
