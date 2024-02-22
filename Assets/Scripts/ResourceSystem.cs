/// <summary>
/// System to manage common player resources (e.g. helath, mana, stamina).
/// </summary>
public class ResourceSystem {
    private int max;
    private int current;

    // Link to UI health bars
    private ResourceBar barUI;

    /// <summary>
    /// Initialize a resource bar with the specified maximum value and link it with the UI.
    /// </summary>
    /// <param name="max">The maximum value for the system to handle.</param>
    /// <param name="barUI">The UI component to link the system to.</param>
    public ResourceSystem(int max, ResourceBar barUI) {
        this.max = max;
        current = max;

        // TODO: make the UI component optional (in case we want hidden health values for lesser enemies)
        this.barUI = barUI;
        barUI.SetMaxValue(max);
    }

    /// <summary>
    /// Removes the specified amount of the resource from the system. The subtraction cannot go under 0.
    /// </summary>
    /// <param name="amount">The amount of damage to deal.</param>
    public void RemoveAmount(int amount) {
        current -= amount;
        if (current < 0) current = 0;

        barUI.SetCurrentValue(current);
    }

    /// <summary>
    /// Adds the specified amount of the resource to the system. The addition cannot exceed its set maximum.
    /// </summary>
    /// <param name="amount">The amount of health to heal.</param>
    public void AddAmount(int amount) {
        current += amount;
        if (current > max) current = max;

        barUI.SetCurrentValue(current);
    }

    // --- GETTERS + SETTERS

    /// <returns>The current value of the resource for this system.</returns>
    public int GetCurrentValue() {
        return current;
    }

    /// <summary>
    ///  Sets the current value of the resource.
    /// </summary>
    /// <param name="newValue">The new value of the resource.</param>
    public void SetCurrentValue(int newValue) {
        current = newValue;

        barUI.SetCurrentValue(current);
    }

    /// <returns>The maximum value of the resource for this system.</returns>
    public int GetMaxValue() {
        return max;
    }

    /// <summary>
    /// Sets the maximum value of the resource.
    /// </summary>
    /// <param name="newMax">The new maximum value of the resource.</param>
    public void SetMaxHealth(int newMax) {
        max = newMax;

        barUI.SetMaxValue(current);
    }
}
