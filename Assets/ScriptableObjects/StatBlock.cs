using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatBlock", menuName = "ScriptableObjects/StatBlock", order = 1)]
public class StatBlock : ScriptableObject {
    [Space(5), Header("Resources"), Space(5)]
    public int baseHealth;
    public int baseMana;
    public int baseStamina;

    [Space(5), Header("Movement"), Space(5)]
    public float baseMoveSpeed;
    public float acceleration;
    public float deceleration;
    public float rollCooldown;

    // General RPG stats
    [Space(5), Header("RPG Stats"), Space(5)]
    public int defense;             // reduces incoming damage
    public int strength;            // determines melee damage
    public int dexterity;           // influences ranged damage, accuracy, critical hit rates
    public int lethality;           // amplifies critical hit damage
    public int intelligence;        // increases spell power
    public int wisdom;              // affects mana regen rate
    public int agility;             // depletes when run / dodge, slowly recharges
}
