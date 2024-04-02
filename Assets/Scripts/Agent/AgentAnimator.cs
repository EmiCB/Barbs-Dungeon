using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimator : MonoBehaviour {
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    public void RotateToPointer(Vector2 pointerDirection) {
        Vector3 scale = transform.localScale;
        if (pointerDirection.x > 0) {
            scale.x = 1;
        }
        else if (pointerDirection.x < 0) {
            scale.x = -1;
        }
        transform.localScale = scale;
    }

    public void PlayWalkAnimation(Vector2 movementInput) {
        animator.SetBool("isWalking", movementInput.magnitude > 0);
    }

    public void PlayRollAnimation() {
        animator.SetTrigger("Roll");
    }
}
