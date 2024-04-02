using UnityEngine;

/// <summary>
/// Class to manage a ganeral enemy. All enemy types should inherit from this class.
/// </summary>
public class EnemyController : MonoBehaviour {
    private Agent agent;
    private WeaponParentController weaponParent;

    public StatBlock statBlock;

    // TODO: make these enemy parameters
    public float chaseDistanceThreshold;    // 5.0
    public float attackDistanceThreshold;   // 2.0
    public float sightDistance;             // 4.0
    public float sightAngle;                // 20.0
    private float passedTime = 0.0f;
    private bool isChasing;

    private Vector2 facingDir;

    private Transform player;

    // --- GAME CONTROL FLOW ---

    private void Start() {
        agent = GetComponent<Agent>();
        weaponParent = GetComponentInChildren<WeaponParentController>();
        player = FindObjectOfType<PlayerController>().transform;

        isChasing = false;
        facingDir = Vector2.zero;
    }

    private void Update() {
        // do nothing if no player
        if (player == null)
            return;

        // get player position info
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);
        Vector2 direction = player.position - transform.position;
        float angleToPlayer = Vector2.Angle(facingDir, direction);

        // check if player in line of sight
        if (angleToPlayer < sightAngle && distanceToPlayer < sightDistance) {
            isChasing = true;
        }

        if (isChasing) {
            // check if player in chase radius
            if (distanceToPlayer < chaseDistanceThreshold ) {
                // look towards player
                agent.aimDirection = player.position;
                facingDir = player.position;

                // attack
                if (distanceToPlayer <= attackDistanceThreshold) {
                    agent.movementDirection = Vector2.zero;

                    if (passedTime >= weaponParent.GetWeaponData().attackCooldown) {
                        passedTime = 0.0f;
                        weaponParent.Attack();
                    }
                }
                
                // move towards player
                else {
                    agent.movementDirection = direction.normalized;
                    
                }
            }

            // stop if lost sight
            else {
                agent.movementDirection = Vector2.zero;
                isChasing = false;
            }
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

    // --- GETTERS + SETTERS ---
    public void SetIsChasing(bool isChasing) {
        this.isChasing = isChasing;
    } 
}
