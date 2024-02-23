using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParentController : MonoBehaviour {
    public SpriteRenderer playerRenderer, weaponRenderer;

    public Vector2 PointerPosition { get; set; }

    private void Update() {
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
}
