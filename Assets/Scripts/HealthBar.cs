using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public Slider slider;

    void Start() {
        slider = gameObject.GetComponent<Slider>();
    }

    /// <summary>
    /// Set the maximum amount of health that the health bar can display.
    /// </summary>
    /// <param name="maxHealth">The maximum amount of health the bar can display.</param>
    public void SetMaxHealth(int maxHealth){
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    /// <summary>
    /// Set the current health for the health bar to display.
    /// </summary>
    /// <param name="currentHealth">The current health to display.</param>
    public void SetCurrentHealth(int currentHealth) {
        slider.value = currentHealth;
    }
}
