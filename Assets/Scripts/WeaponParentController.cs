using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponParentController : MonoBehaviour {
    public PlayerController playerController;
    public SpriteRenderer playerRenderer, weaponRenderer;

    public Vector2 PointerPosition { get; set; }

    // Set up animnation
    public Animator animator;

    // TODO: move this?
    public float delay = 0.3f;
    private bool isAttacking = false;
    public bool IsAttacking { get; private set; }

    // Raycast detection
    public Transform raycastOrigin;
    public float raycastRadius;

    public void ResetIsAttacking() {
        IsAttacking = false;
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

    public void Attack() {
        // Stop attack if already attacking
        if (isAttacking) { return; }

        // Trigger attack animation
        animator.SetTrigger("Attack");
        IsAttacking = true;
        isAttacking = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack() {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    public void DetectColliders() {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(raycastOrigin.position, raycastRadius)) {
            // Player can't hit themselves
            if (collider.tag == "Player") { continue; }

            // Deal damage to enemies
            EnemyController enemyController = collider.GetComponent<EnemyController>();
            if (enemyController != null) {
                enemyController.ApplyDamage(playerController.getBaseDamage());
            }

        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Vector3 position = raycastOrigin == null ? Vector3.zero : raycastOrigin.position;
        Gizmos.DrawWireSphere(position, raycastRadius);
    }
}
