using System.Collections;
using UnityEngine;

/// <summary>
/// Class to control the Weapon Parent object. This allows for general weapon control
/// and easy swapping of the specific weapon beaing used.
/// </summary>
public class WeaponParentController : MonoBehaviour {
    private SpriteRenderer agentRenderer, weaponRenderer;
    private WeaponData weaponData;

    public ObjectPooler projectilePool;

    public Vector2 PointerPosition { get; set; }

    // Set up animation
    private Animator animator;

    // Weapon cooldown
    private bool isAttackInProgress = false;
    public bool IsAttacking { get; private set; }

    // Raycast detection
    public Transform raycastOrigin;
    public float raycastRadius;

    public Transform projectileOrigin;

    [SerializeField] AudioClip bowClip;

    private void Start() {
        // Automatically get components for this script
        agentRenderer = transform.parent.gameObject.GetComponentInChildren<SpriteRenderer>();
        UpdateEquippedWeapon();
    }

    private void Update() {
        // Do not rotate if in attack animation
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
            weaponRenderer.sortingOrder = agentRenderer.sortingOrder - 1;
        } else {
            weaponRenderer.sortingOrder = agentRenderer.sortingOrder + 1;
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
        if (weaponData.weaponType == WeaponType.Ranged && weaponData.projectilePrefab != null) {
            GameObject projectile = projectilePool.GetPooledObject();
            projectile.transform.position = projectileOrigin.position;
            projectile.transform.rotation = weaponData.projectilePrefab.transform.rotation * transform.rotation;
            projectile.SetActive(true);

            SoundFXManager.instance.PlaySoundFXClip(bowClip, transform, 0.5f);
        }
    }

    /// <summary>
    /// Disable attacking again until delay period is over.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayAttack() {
        Agent agent = GetComponentInParent<Agent>();
        yield return new WaitForSeconds(weaponData.attackCooldown * agent.statBlock.attackSpeedMod);
        isAttackInProgress = false;
    }

    /// <summary>
    /// Handle collider collisions.
    /// TODO: clean this up and make more flexible
    /// </summary>
    public void DetectColliders() {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(raycastOrigin.position, raycastRadius)) {
            // Get users and targets
            PlayerController playerUser = GetComponentInParent<PlayerController>();
            EnemyController enemyUser = GetComponentInParent<EnemyController>();
            PlayerController playerTarget = collider.GetComponent<PlayerController>();
            EnemyController enemyTarget = collider.GetComponent<EnemyController>();

            // User can't hit themselves
            if (collider.tag == "Player" && playerUser != null) { continue; }
            if (collider.tag == "Enemy" && enemyUser != null) { continue; }

            // Deal damage to target
            if (playerTarget != null) {
                // Roll i-frames
                if (playerTarget.IsRollOnCooldown) { continue; }
                playerTarget.ApplyDamage(weaponData.baseDamage);
            }
            if (enemyTarget != null) {
                enemyTarget.ApplyDamage(weaponData.baseDamage);
            }
        }
    }

    /// <summary>
    /// Update the equipped weapon references.
    /// </summary>
    public void UpdateEquippedWeapon() {
        weaponRenderer = GetComponentInChildren<SpriteRenderer>();
        weaponData = GetComponentInChildren<WeaponController>().weaponData;
        animator = GetComponentInChildren<Animator>();

        // Set up projectile pool based on weapon
        if (weaponData.weaponType == WeaponType.Ranged && weaponData.projectilePrefab != null) {
            projectilePool.pooledObject = weaponData.projectilePrefab;
            projectilePool.pooledAmount = 5;
        }
    }

    // --- DEBUG ---

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
