using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawLinesBetweenObjects : MonoBehaviour
{
    public Transform[] objects;
    public GameObject targetObject;

    private LineRenderer[] lineRenderers;

    void Start()
    {
        int numberOfLines = objects.Length / 2;
        lineRenderers = new LineRenderer[numberOfLines];

        for (int i = 0; i < numberOfLines; i++)
        {
            GameObject lineObject = new GameObject("Line" + i);
            lineRenderers[i] = lineObject.AddComponent<LineRenderer>();

            lineRenderers[i].positionCount = 2;
            lineRenderers[i].startWidth = 0.002f;
            lineRenderers[i].endWidth = 0.002f;

            if (targetObject.activeSelf)
            {
                lineRenderers[i].startColor = Color.black;
                lineRenderers[i].endColor = Color.black;
            }
            else
            {
                lineRenderers[i].startColor = new Color(1, 1, 1, 0);
                lineRenderers[i].endColor = new Color(1, 1, 1, 0);
            }

            lineRenderers[i].material = new Material(Shader.Find("Unlit/Color"));
        }
    }

    void Update()
    {
        bool isTargetActive = targetObject.activeSelf;

        for (int i = 0; i < objects.Length; i += 2)
        {

            if (objects[i] != null && objects[i + 1] != null)
            {
                int lineIndex = i / 2;

                if (isTargetActive)
                {
                    lineRenderers[lineIndex].startColor = Color.black;
                    lineRenderers[lineIndex].endColor = Color.black;
                    lineRenderers[lineIndex].SetPosition(0, objects[i].position);
                    lineRenderers[lineIndex].SetPosition(1, objects[i + 1].position);
                }
                else
                {
                    lineRenderers[lineIndex].startColor = new Color(1, 1, 1, 0);
                    lineRenderers[lineIndex].endColor = new Color(1, 1, 1, 0);
                    lineRenderers[lineIndex].SetPosition(0, Vector3.zero);
                    lineRenderers[lineIndex].SetPosition(1, Vector3.zero);
                }
            }
        }
    }
}