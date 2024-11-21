using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;  // The target (usually the player) for the camera to follow
    public Vector3 offset = new Vector3(0, 1, -10);  // The offset of the camera relative to the player
    public float smoothSpeed = 0.125f;  // How smoothly the camera follows the player

    void FixedUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position based on the target position and the offset
            Vector3 desiredPosition = target.position + offset;
            desiredPosition.y = transform.position.y;
            // Smoothly interpolate between the camera's current position and the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Apply the smoothed position to the camera
            transform.position = smoothedPosition;
        }
    }
}
