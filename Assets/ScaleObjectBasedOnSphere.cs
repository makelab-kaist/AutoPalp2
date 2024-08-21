using UnityEngine;

public class ScaleObjectBasedOnSphere : MonoBehaviour
{
    public GameObject sphere; // The sphere that will be grabbed and moved
    public GameObject targetObject; // The object whose scale will be adjusted
    public string axisToScale = "x"; // The axis to scale: "x", "y", or "z"
    public float minScale = 0.1f; // Minimum scale factor
    public float maxScale = 3f; // Maximum scale factor
    public float zMin = -10f; // Minimum Z position of the sphere
    public float zMax = 10f; // Maximum Z position of the sphere
    public float scalingFactor = 2f; // Power factor to make scaling more sensitive

    private Vector3 initialScale; // Initial scale of the target object

    void Start()
    {
        if (targetObject != null)
        {
            initialScale = targetObject.transform.localScale; // Store the initial scale
        }
    }

    void Update()
    {
        if (sphere != null && targetObject != null)
        {
            float zPosition = sphere.transform.position.x; // Get the sphere's current Z position
            float t = Mathf.InverseLerp(zMin, zMax, zPosition); // Normalize Z position between 0 and 1
            t = Mathf.Pow(t, scalingFactor); // Apply the scaling factor for non-linear scaling
            float scaleValue = Mathf.Lerp(minScale, maxScale, t); // Interpolate the scale value

            Vector3 newScale = initialScale; // Start with the initial scale

            // Apply the scale value to the specified axis
            switch (axisToScale.ToLower())
            {
                case "x":
                    newScale.x = scaleValue;
                    break;
                case "y":
                    newScale.y = scaleValue;
                    break;
                case "z":
                    newScale.z = scaleValue;
                    break;
                default:
                    Debug.LogWarning("Invalid axis specified for scaling.");
                    break;
            }

            // Set the new scale to the target object
            targetObject.transform.localScale = newScale;
        }
    }
}
