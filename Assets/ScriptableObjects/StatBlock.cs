using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatBlock", menuName = "ScriptableObjects/StatBlock", order = 1)]
public class StatBlock : ScriptableObject {
    // Resouces
    public int baseHealth;
    public int baseMana;

    // Movement
    public float baseMoveSpeed;
    public float acceleration;
    public float deceleration;
    public float rollCooldown;

    // General RPG stats
    // TODO: fill in
}
