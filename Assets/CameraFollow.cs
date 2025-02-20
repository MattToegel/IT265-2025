using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;  // Assign Player in Inspector
    public Vector3 offset = new Vector3(0, 5, -5);  // Adjust for Third-Person
    public float smoothSpeed = 5f;  // Camera follow speed
    public float rotationSpeed = 5f; // Smooth rotation speed
    public float manualRotationSpeed = 100f; // Speed for manual rotation
    private float manualRotationY = 0f; // Stores manual rotation

    private bool isManuallyRotating = false; // Tracks if manual rotation is active

    private void Awake() { 
        if (target == null) {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void LateUpdate() {
        if (target == null) return;

        // 🔹 Handle Manual Camera Rotation (Arrow Keys)
        float horizontalInput = Input.GetAxis("Horizontal"); // Left/Right Arrow or A/D
        if (horizontalInput != 0) {
            isManuallyRotating = true; // Manual rotation is active
            manualRotationY += horizontalInput * manualRotationSpeed * Time.deltaTime;
        }

        // 🔹 Detect when player moves to reset manual rotation
        if (Input.GetAxis("Vertical") != 0 || Input.GetMouseButtonDown(0)) {
            isManuallyRotating = false; // Reset to automatic rotation
        }

        // 🔹 Determine the final rotation
        Quaternion desiredRotation;
        if (isManuallyRotating) {
            // Use manually adjusted Y rotation
            desiredRotation = Quaternion.Euler(0, manualRotationY, 0);
        } else {
            // Rotate the container (CameraFollow) on Y-axis only following the player
            Vector3 forwardDirection = target.forward;
            forwardDirection.y = 0; // Keep rotation level
            if (forwardDirection != Vector3.zero) {
                desiredRotation = Quaternion.LookRotation(forwardDirection);
                manualRotationY = desiredRotation.eulerAngles.y; // Sync manual rotation when resetting
            } else {
                desiredRotation = transform.rotation;
            }
        }

        // Smoothly rotate the camera follow container
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);

        // 🔹 Maintain correct relative position
        Vector3 desiredPosition = target.position + transform.rotation * offset;

        // Smoothly move towards the correct position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
