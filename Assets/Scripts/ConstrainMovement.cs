using UnityEngine;

public class ConstrainMovement : MonoBehaviour
{
    public Vector3 allowedDirection = Vector3.right; // Set this to the direction you want to allow, e.g., Vector3.right for only X-axis

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        // Calculate the allowed movement by projecting onto the allowed direction
        Vector3 movement = Vector3.Project(transform.position - lastPosition, allowedDirection);

        // Update the position to be the last position plus the allowed movement
        transform.position = lastPosition + movement;

        // Update the last position for the next frame
        lastPosition = transform.position;
    }
}