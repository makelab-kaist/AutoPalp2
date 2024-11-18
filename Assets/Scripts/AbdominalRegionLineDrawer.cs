using UnityEngine;

/// <summary>
/// Draws lines between pairs of objects to represent 9 abdominal palpation regions.
/// The visibility of the lines depends on the active state of the target object.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class AbdominalRegionLineDrawer : MonoBehaviour
{
    /// <summary>
    /// Array of transforms representing points between which lines will be drawn.
    /// Must contain pairs of start and end points in sequence.
    /// </summary>
    public Transform[] lineEndpoints;

    /// <summary>
    /// The GameObject whose active state determines the visibility of the lines.
    /// </summary>
    public GameObject visibilityController;

    /// <summary>
    /// Array of LineRenderer components used to draw lines between the specified endpoints.
    /// </summary>
    private LineRenderer[] lineRenderers;

    void Start()
    {
        // Calculate the number of lines to be drawn based on the number of endpoint pairs.
        int numberOfLines = lineEndpoints.Length / 2;
        lineRenderers = new LineRenderer[numberOfLines];

        // Loop to initialize LineRenderers for each line.
        for (int i = 0; i < numberOfLines; i++)
        {
            // Create a new GameObject to hold the LineRenderer for this line.
            GameObject lineObject = new GameObject("Line" + i);
            lineRenderers[i] = lineObject.AddComponent<LineRenderer>();

            // Add a LineRenderer component to the GameObject and configure it.
            lineRenderers[i].positionCount = 2; // A line requires two points.
            lineRenderers[i].startWidth = 0.002f; // Set the width of the line.
            lineRenderers[i].endWidth = 0.002f;

            // Set the line's color and material based on the visibility controller's state.
            if (visibilityController.activeSelf)
            {
                lineRenderers[i].startColor = Color.black;
                lineRenderers[i].endColor = Color.black;
            }
            else
            {
                lineRenderers[i].startColor = new Color(1, 1, 1, 0); // Transparent.
                lineRenderers[i].endColor = new Color(1, 1, 1, 0); // Transparent.
            }

            // Use an unlit material to ensure consistent line color regardless of lighting.
            lineRenderers[i].material = new Material(Shader.Find("Unlit/Color"));
        }
    }

    void Update()
    {
        // Determine if the visibility controller is active.
        bool isControllerActive = visibilityController.activeSelf;

        // Iterate through the endpoint array in pairs.
        for (int i = 0; i < lineEndpoints.Length; i += 2)
        {
            if (lineEndpoints[i] != null && lineEndpoints[i + 1] != null)
            {
                // Calculate the index of the corresponding LineRenderer.
                int lineIndex = i / 2;

                if (isControllerActive)
                {
                    // Make the line visible and update its positions.
                    lineRenderers[lineIndex].startColor = Color.black;
                    lineRenderers[lineIndex].endColor = Color.black;
                    lineRenderers[lineIndex].SetPosition(0, lineEndpoints[i].position);
                    lineRenderers[lineIndex].SetPosition(1, lineEndpoints[i + 1].position);
                }
                else
                {
                    // Make the line invisible and reset its positions.
                    lineRenderers[lineIndex].startColor = new Color(1, 1, 1, 0);
                    lineRenderers[lineIndex].endColor = new Color(1, 1, 1, 0);
                    lineRenderers[lineIndex].SetPosition(0, Vector3.zero);
                    lineRenderers[lineIndex].SetPosition(1, Vector3.zero);
                }
            }
        }
    }
}