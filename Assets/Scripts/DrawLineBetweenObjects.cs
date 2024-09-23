using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawLinesBetweenObjects : MonoBehaviour
{
    public Transform[] objects; // Array to store objects (must be in pairs)

    private LineRenderer[] lineRenderers;

    void Start()
    {
        int numberOfLines = objects.Length / 2; // We need one line per pair of objects
        lineRenderers = new LineRenderer[numberOfLines];

        for (int i = 0; i < numberOfLines; i++)
        {
            // Create a new GameObject for each line and attach a LineRenderer component
            GameObject lineObject = new GameObject("LineRenderer_" + i);
            lineRenderers[i] = lineObject.AddComponent<LineRenderer>();

            // Set the number of positions (2 since we are connecting two objects)
            lineRenderers[i].positionCount = 2;

            lineRenderers[i].startColor = Color.black;
            lineRenderers[i].endColor = Color.black;

            lineRenderers[i].material = new Material(Shader.Find("Unlit/Color"));
            lineRenderers[i].material.color = Color.black;

            // Set the width of the line (optional)
            lineRenderers[i].startWidth = 0.002f;
            lineRenderers[i].endWidth = 0.002f;
        }
    }

    void Update()
    {
        for (int i = 0; i < objects.Length; i += 2)
        {
            if (objects[i] != null && objects[i + 1] != null)
            {
                int lineIndex = i / 2;
                // Set the positions of the line to the positions of the paired objects
                lineRenderers[lineIndex].SetPosition(0, objects[i].position);   // Start of the line (object[i])
                lineRenderers[lineIndex].SetPosition(1, objects[i + 1].position); // End of the line (object[i+1])
            }
        }
    }
}