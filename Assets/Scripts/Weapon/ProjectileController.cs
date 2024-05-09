using UnityEngine;

public class ProjectileController : MonoBehaviour {
    private Rigidbody2D rb2d;

    private WeaponParentController weaponParent;
    private WeaponData weaponData;

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        weaponParent = FindObjectOfType<WeaponParentController>();
        weaponData = weaponParent.GetWeaponData();
    }

    void FixedUpdate() {
        rb2d.AddForce(transform.up * weaponData.projectileForce, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D other) {
        // Player can't hit themselves
        if (other.tag == "Player") { return; }

        // Deal damage to enemies
        Agent enemyController = other.GetComponent<Agent>();
        if (enemyController != null) {
            enemyController.ApplyDamage(weaponData.baseDamage);
        }

        gameObject.SetActive(false);
    }

    // TODO: might need to change this if we want to hit enemies off screen
    void OnBecameInvisible() {
        gameObject.SetActive(false);
    }
}
