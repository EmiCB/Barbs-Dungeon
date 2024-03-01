using UnityEngine;

public class ProjectileController : MonoBehaviour {
    public float projectileForce;
    private Rigidbody2D rb2d;

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        rb2d.AddForce(transform.up * projectileForce, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D other) {
        // Player can't hit themselves
        if (other.tag == "Player") { return; }

        // Deal damage to enemies
        EnemyController enemyController = other.GetComponent<EnemyController>();
        if (enemyController != null) {
            enemyController.ApplyDamage(1);
        }

        gameObject.SetActive(false);
    }
}
