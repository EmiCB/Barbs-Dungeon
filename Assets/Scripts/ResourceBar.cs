using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to control what a UI slider displays from a script.
/// </summary>
public class ResourceBar : MonoBehaviour {
    public Slider slider;

    void Start() {
        slider = gameObject.GetComponent<Slider>();
    }

    /// <summary>
    /// Set the maximum value that the resource bar can display.
    /// </summary>
    /// <param name="max">The maximum value that the bar can display.</param>
    public void SetMaxValue(int max){
        slider.maxValue = max;
        slider.value = max;
    }

    /// <summary>
    /// Set the current value for the resource bar to display.
    /// </summary>
    /// <param name="current">The current value to display.</param>
    public void SetCurrentValue(int current) {
        slider.value = current;
    }
}
