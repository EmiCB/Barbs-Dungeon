using UnityEngine;

public enum WeaponType {
    Melee,
    Ranged
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject {
    public WeaponType weaponType;

    public int baseDamage;
    public float attackCooldown;

    // projectile settings FOR RANGED WEAPONS ONLY
    public GameObject projectilePrefab;
    public float projectileForce;
}
