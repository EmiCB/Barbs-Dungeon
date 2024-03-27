using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMover : MonoBehaviour {
    private Rigidbody2D rb2d;
    private StatBlock statBlock;

    // Movement variables
    private float currentMoveSpeed = 0.0f;
    public Vector2 movementInput = Vector2.zero;
    private Vector2 movementInputOld = Vector2.zero;

    void Awake() {
        // Find unassigned components
        rb2d = GetComponent<Rigidbody2D>();
        statBlock = GetComponent<Agent>().statBlock;
    }

    void FixedUpdate() {
        // Make smoother movement
        if (movementInput.magnitude > 0 && currentMoveSpeed >= 0) {
            movementInputOld = movementInput;
            currentMoveSpeed += statBlock.acceleration * statBlock.baseMoveSpeed * Time.deltaTime;
        } else {
            currentMoveSpeed -= statBlock.deceleration * statBlock.baseMoveSpeed * Time.deltaTime;
        }
        currentMoveSpeed = Mathf.Clamp(currentMoveSpeed, 0, statBlock.baseMoveSpeed);
        rb2d.velocity = movementInputOld * currentMoveSpeed;
    }
}
