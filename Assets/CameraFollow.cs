using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;  // Assign Player in Inspector
    public Vector3 offset = new Vector3(0, 5, -5);  // Adjust for Third-Person
    public float smoothSpeed = 5f;  // Camera follow speed
    public float rotationSpeed = 5f; // Smooth rotation speed

    private void Awake() {
        if (target == null) {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void LateUpdate() {
        if (target == null) return;

        // Rotate the container (CameraFollow) on Y-axis only
        Vector3 forwardDirection = target.forward;
        forwardDirection.y = 0; // Keep rotation level
        if (forwardDirection != Vector3.zero) // Prevent NaN rotation errors
        {
            Quaternion desiredRotation = Quaternion.LookRotation(forwardDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }

        // Maintain the camera’s correct relative position
        Vector3 desiredPosition = target.position + transform.rotation * offset;

        // Smoothly move towards the correct position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
