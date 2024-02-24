using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private int maxHealth = 2;
    private ResourceSystem healthSystem;

    // Start is called before the first frame update
    void Start() {
        healthSystem = new ResourceSystem(maxHealth, null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyDamage(int amount) {
        healthSystem.RemoveAmount(amount);
        if (healthSystem.GetCurrentValue() <= 0) {
            gameObject.SetActive(false);
        }
    }
}
