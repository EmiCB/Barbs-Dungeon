using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    // Movement variables
    private float moveSpeed = 10.0f;
    private Vector2 movementInput = Vector2.zero;

    // Component references
    private Rigidbody2D rb;

    // Initialize player
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Run physics
    private void FixedUpdate() {
        rb.velocity = movementInput * moveSpeed;
    }

    /// <summary>
    /// Responds to OnMove event from UnityInputSystem.
    /// </summary>
    /// <param name="value">The action value that contains the Vector2 X/Y input from the player's input device.</param>
    void OnMove(InputValue value) {
        movementInput = value.Get<Vector2>();
    }
}
