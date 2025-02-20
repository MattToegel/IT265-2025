using UnityEngine;
using UnityEngine.AI;

public class PlayerClickToMove : MonoBehaviour {
    public NavMeshAgent agent;
    public Camera mainCamera;
    public LayerMask groundLayer;  // Assign "Ground" layer in Inspector
    public LayerMask interactableLayer; // Assign "Interactable" layer in Inspector
    public float maxNavMeshSearchDistance = 0.5f; // Max distance to find a valid position near the object

    void Start() {
        if (mainCamera == null) mainCamera = Camera.main;
        if (agent == null) agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) // Left Click
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform a raycast that detects BOTH ground and interactables
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer | interactableLayer)) {
                // Check if we hit an interactable object FIRST
                if (((1 << hit.collider.gameObject.layer) & interactableLayer) != 0) {
                    MoveNearObject(hit.point);
                }
                else if (((1 << hit.collider.gameObject.layer) & groundLayer) != 0) {
                    // If we hit the ground, move directly there
                    agent.SetDestination(hit.point);
                }
            }
        }
    }

    void MoveNearObject(Vector3 objectPosition) {
        NavMeshHit navHit;

        // Find a valid point on the NavMesh within maxNavMeshSearchDistance from the object
        if (NavMesh.SamplePosition(objectPosition, out navHit, maxNavMeshSearchDistance, NavMesh.AllAreas)) {
            agent.SetDestination(navHit.position);
        }
        else {
            Debug.Log("No valid NavMesh position found near object.");
        }
    }
}
