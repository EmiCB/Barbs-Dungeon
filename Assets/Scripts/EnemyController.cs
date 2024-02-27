using UnityEngine;

/// <summary>
/// Class to manage a ganeral enemy. All enemy types should inherit from this class.
/// </summary>
public class EnemyController : MonoBehaviour {
    // Health system
    private int maxHealth = 2;              // TODO: split out into a enemy stats ScriptableObject
    private ResourceSystem healthSystem;

    void Start() {
        // Initialize stats
        healthSystem = new ResourceSystem(maxHealth, null);
    }

    /// <summary>
    /// Apply damage to this enemy.
    /// </summary>
    /// <param name="amount"></param>
    public void ApplyDamage(int amount) {
        healthSystem.RemoveAmount(amount);

        // Check if enemy is dead and remove it from the scene.
        if (healthSystem.GetCurrentValue() <= 0) {
            // TODO: make ObjectPooler for enemies to help increase performance + reduce possible
            // memory leaks
            gameObject.SetActive(false);
        }
    }
}
