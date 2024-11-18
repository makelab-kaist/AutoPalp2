using UnityEngine;

/// <summary>
/// Controls the size of the abdomen UI element by adjusting its scale based on the positions of size adjustment handles.
/// </summary>
public class AbdomenUISizeController : MonoBehaviour
{
    /// <summary>
    /// The abdomen GameObject whose size is being adjusted.
    /// </summary>
    public GameObject abdomenObject;

    /// <summary>
    /// The GameObject used to adjust the width (X-axis) of the abdomen.
    /// </summary>
    public GameObject widthAdjustHandle;

    /// <summary>
    /// The GameObject used to adjust the height (Y-axis) of the abdomen.
    /// </summary>
    public GameObject heightAdjustHandle;

    /// <summary>
    /// The initial scale of the abdomen at the start of the application.
    /// </summary>
    private Vector3 initialScale;

    /// <summary>
    /// The initial distance between the width adjustment handle and the abdomen.
    /// </summary>
    private float initialWidthDistance;

    /// <summary>
    /// The initial distance between the height adjustment handle and the abdomen.
    /// </summary>
    private float initialHeightDistance;

    void Start()
    {
        // Store the initial scale of the abdomen.
        initialScale = abdomenObject.transform.localScale;
        
        // Store the initial distances between the abdomen and the adjustment handles.
        initialWidthDistance = Vector3.Distance(widthAdjustHandle.transform.position, abdomenObject.transform.position);
        initialHeightDistance = Vector3.Distance(heightAdjustHandle.transform.position, abdomenObject.transform.position);
    }

    void Update()
    {
        // Create a new scale vector based on the initial scale.
        Vector3 updatedScale = initialScale;

        // Calculate the current distances between the adjustment handles and the abdomen.
        float updatedDistanceX = Vector3.Distance(widthAdjustHandle.transform.position, abdomenObject.transform.position);
        float updatedDistanceY = Vector3.Distance(heightAdjustHandle.transform.position, abdomenObject.transform.position);

        // Adjust the X and Y scale of the abdomen based on the ratio of current to initial distances.
        updatedScale.x = initialScale.x * updatedDistanceX / initialWidthDistance;
        updatedScale.y = initialScale.y * updatedDistanceY / initialHeightDistance;

        // Apply the updated scale to the abdomen.
        abdomenObject.transform.localScale = updatedScale;
    }
}
