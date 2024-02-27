using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Helper class to trigger events from Animations.
/// </summary>
public class AnimationEventHelper : MonoBehaviour {
    // TODO: rewrite this class to make it more extensible

    public UnityEvent OnAnimationEventTriggered, OnAttackPerformed;

    public void TriggerEvent() {
        OnAnimationEventTriggered?.Invoke();
    }
    
    public void TriggerAttack() {
        OnAttackPerformed?.Invoke();
    }
}
