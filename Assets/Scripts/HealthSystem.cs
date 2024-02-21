public class HealthSystem {
    private int maxHealth;
    private int currentHealth;

    // Link to UI health bars
    private HealthBar healthBar;

    public HealthSystem(int maxHealth, HealthBar healthBar) {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;

        this.healthBar = healthBar;
        healthBar.SetMaxHealth(maxHealth);
    }

    /// <summary>
    /// Removes the specified amount of health from the entity. The subtraction cannot go under 0.
    /// </summary>
    /// <param name="amount">The amount of damage to deal.</param>
    public void ApplyDamage(int amount) {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        healthBar.SetCurrentHealth(currentHealth);
    }

    /// <summary>
    /// Adds the specified amount of health to the entity. The addition cannot exceed their max health.
    /// </summary>
    /// <param name="amount">The amount of health to heal.</param>
    public void ApplyHeal(int amount) {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        healthBar.SetCurrentHealth(currentHealth);
    }

    /// <returns>The current health of this entity.</returns>
    public int GetHealth() {
        return currentHealth;
    }
}
