using UnityEngine;

/// <summary>
/// Class to manage a ganeral enemy. All enemy types should inherit from this class.
/// </summary>
public class EnemyController : MonoBehaviour {
    private Agent agent;
    private WeaponParentController weaponParent;

    public StatBlock statBlock;

    // TODO: make these enemy parameters
    public float chaseDistanceThreshold = 5.0f;
    public float attackDistanceThreshold = 2.0f;
    private float passedTime = 0.0f;

    private Transform player;

    // --- GAME CONTROL FLOW ---

    private void Start() {
        agent = GetComponent<Agent>();
        weaponParent = GetComponentInChildren<WeaponParentController>();
        player = FindObjectOfType<PlayerController>().transform;
    }

    private void Update() {
        // do nothing if no player
        if (player == null)
            return;

        float distanceToPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceToPlayer < chaseDistanceThreshold ) {
            // look towards player
            agent.aimDirection = player.position;

            // attack? or move
            if (distanceToPlayer <= attackDistanceThreshold) {
                agent.movementDirection = Vector2.zero;

                if (passedTime >= weaponParent.GetWeaponData().attackCooldown) {
                    passedTime = 0.0f;
                    weaponParent.Attack();
                }
            }
            
            else {
                Vector2 direction = player.position - transform.position;
                agent.movementDirection = direction.normalized;
            }
        }

        else {
            // stop if lost sight
            agent.movementDirection = Vector2.zero;
        }

        // update timed variables
        if (passedTime < weaponParent.GetWeaponData().attackCooldown) {
            passedTime += Time.deltaTime;
        }
    }

    // --- FUNCTIONS --

    /// <summary>
    /// Apply damage to this enemy.
    /// </summary>
    /// <param name="amount"></param>
    public void ApplyDamage(int amount) {
        agent.healthSystem.RemoveAmount(amount);

        // Check if enemy is dead and remove it from the scene.
        if (agent.healthSystem.GetCurrentValue() <= 0) {
            // TODO: make ObjectPooler for enemies to help increase performance + reduce possible
            // memory leaks
            gameObject.SetActive(false);
        }
    }
}
