using UnityEngine;

public class RestrictXAxis : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Get the current position of the GameObject
        Vector3 currentPosition = transform.position;

        // Clamp the x position between -0.15 and 0.05
        currentPosition.x = Mathf.Clamp(currentPosition.x, -0.15f, 0.05f);

        // Apply the new clamped position back to the GameObject
        transform.position = currentPosition;
    }
}