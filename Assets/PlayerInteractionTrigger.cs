using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionTrigger : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out Interactable interactable)) {
            interactable.Enable();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.TryGetComponent(out Interactable interactable)) {
            interactable.Disable();
        }
    }
}

