using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems; // Required for UI checks

public class PlayerClickToMove : MonoBehaviour {
    public NavMeshAgent agent;
    public Camera mainCamera;
    public LayerMask groundLayer;  // Assign "Ground" layer in Inspector
    public LayerMask interactableLayer; // Assign "Interactable" layer in Inspector
    public LayerMask blockingLayer; // Assign "Blocking" layers (e.g., walls, UI)
    public float maxNavMeshSearchDistance = 0.5f; // Max distance to find a valid position near an object

    void Start() {
        if (mainCamera == null) mainCamera = Camera.main;
        if (agent == null) agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) // Left Click
        {
            // 🔹 STEP 1: Check if the click is on a UI element
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) {
                Debug.Log("[UI BLOCKED] Click was on a UI element, ignoring movement.");
                return; // Stop processing movement if clicking UI
            }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Debug.Log("=== Click Event Started ===");

            // 🔹 STEP 2: FIRST, check if the ray hits a BLOCKING layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, blockingLayer)) {
                Debug.Log($"[BLOCKED] Click hit {hit.collider.gameObject.name} on layer {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
                return; // Prevents movement if the ray hits a blocking layer
            }

            // 🔹 STEP 3: Perform raycast to detect interactables and ground, excluding blocking layers
            int combinedLayerMask = groundLayer | interactableLayer;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, combinedLayerMask)) {
                int hitLayer = hit.collider.gameObject.layer;
                string hitLayerName = LayerMask.LayerToName(hitLayer);
                Debug.Log($"[HIT] Click detected {hit.collider.gameObject.name} on layer {hitLayerName}");

                if (((1 << hitLayer) & interactableLayer) != 0) {
                    Debug.Log($"[INTERACTABLE] Moving near {hit.collider.gameObject.name}");
                    MoveNearObject(hit.point);
                    return; // Exit early to avoid ground movement
                }

                if (((1 << hitLayer) & groundLayer) != 0) {
                    Debug.Log($"[GROUND] Moving to {hit.point}");
                    agent.SetDestination(hit.point);
                }
            } else {
                Debug.Log("[NO HIT] Click didn't hit any valid layer.");
            }
        }
    }

    void MoveNearObject(Vector3 objectPosition) {
        NavMeshHit navHit;

        // Find a valid point on the NavMesh within maxNavMeshSearchDistance from the object
        if (NavMesh.SamplePosition(objectPosition, out navHit, maxNavMeshSearchDistance, NavMesh.AllAreas)) {
            Debug.Log($"[NAVMESH] Moving near interactable at: {navHit.position}");
            agent.SetDestination(navHit.position);
        } else {
            Debug.Log("[NAVMESH] No valid NavMesh position found near object.");
        }
    }
}
