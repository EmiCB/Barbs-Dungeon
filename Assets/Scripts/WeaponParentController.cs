using System.Collections;
using UnityEngine;

// TODO: for later, add a function to update weapon data + display sprite on equip

/// <summary>
/// Class to control the Weapon Parent object. This allows for general weapon control
/// and easy swapping of the specific weapon beaing used.
/// </summary>
public class WeaponParentController : MonoBehaviour {
    private PlayerController playerController;
    private WeaponController weaponController;
    private SpriteRenderer playerRenderer, weaponRenderer;
    private WeaponData weaponData;

    public Vector2 PointerPosition { get; set; }

    // Set up animnation
    private Animator animator;

    // Weapon cooldown
    private bool isAttackInProgress = false;
    public bool IsAttacking { get; private set; }

    // Raycast detection
    public Transform raycastOrigin;
    public float raycastRadius;

    public Transform projectileOrigin;

    private void Start() {
        // automaticall get components for this script
        playerController = GetComponentInParent<PlayerController>();
        weaponController = GetComponentInChildren<WeaponController>();
        playerRenderer = playerController.GetComponentInChildren<SpriteRenderer>();
        weaponRenderer = GetComponentInChildren<SpriteRenderer>();
        weaponData = GetComponentInChildren<WeaponController>().weaponData;
        animator = GetComponentInChildren<Animator>();
    }

    private void Update() {
        // Do not rotate if in att6ack animation
        if (IsAttacking) { return; }

        // Rotate based on cursor position
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        // Flip weapon (so it doesnt keep spinning)
        Vector2 scale = transform.localScale;
        if (direction.x < 0) {
            scale.y = -1;
        } else if (direction.x > 0) {
            scale.y = 1;
        }
        transform.localScale = scale;

        // Hide weapon behind player if rotating above head
        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180) {
            weaponRenderer.sortingOrder = playerRenderer.sortingOrder - 1;
        } else {
            weaponRenderer.sortingOrder = playerRenderer.sortingOrder + 1;
        }
    }

    /// <summary>
    /// Resets the IsAttacking property.
    /// </summary>
    public void ResetIsAttacking() {
        IsAttacking = false;
    }

    /// <summary>
    /// Perform an attack.
    /// </summary>
    public void Attack() {
        // Stop attack if already attacking
        if (isAttackInProgress) { return; }

        // Trigger attack animation
        animator.SetTrigger("Attack");
        IsAttacking = true;
        isAttackInProgress = true;
        StartCoroutine(DelayAttack());

        // create projectile if ranged
        if (weaponData.projectilePrefab != null) {
            Instantiate(weaponData.projectilePrefab, 
                        projectileOrigin.position, 
                        weaponData.projectilePrefab.transform.rotation * transform.rotation);
        }
    }

    /// <summary>
    /// Disable attacking again until delay period is over.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayAttack() {
        yield return new WaitForSeconds(weaponData.attackCooldown);
        isAttackInProgress = false;
    }

    /// <summary>
    /// Handle collider collisions.
    /// </summary>
    public void DetectColliders() {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(raycastOrigin.position, raycastRadius)) {
            // Player can't hit themselves
            if (collider.tag == "Player") { continue; }

            // Deal damage to enemies
            EnemyController enemyController = collider.GetComponent<EnemyController>();
            if (enemyController != null) {
                enemyController.ApplyDamage(weaponData.baseDamage);
            }
        }
    }

    /// <summary>
    /// Draw gizmos to help visualize in the editor.
    /// </summary>
    private void OnDrawGizmosSelected() {
        // Display raycats attack circle
        Gizmos.color = Color.red;
        Vector3 position = raycastOrigin == null ? Vector3.zero : raycastOrigin.position;
        Gizmos.DrawWireSphere(position, raycastRadius);
    }

    // --- GETTERS + SETTERS ---
    public WeaponData GetWeaponData() {
        return weaponData;
    }
}
