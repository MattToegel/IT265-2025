using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {
    [Header("Events")]
    public UnityEvent onEnable;
    public UnityEvent onDisable;

    public void Enable() {
        Debug.Log($"{gameObject.name} Enabled");
        onEnable.Invoke(); // Trigger any assigned Unity Event in the Inspector
    }

    public void Disable() {
        Debug.Log($"{gameObject.name} Disabled");
        onDisable.Invoke(); // Trigger any assigned Unity Event in the Inspector
    }
}
