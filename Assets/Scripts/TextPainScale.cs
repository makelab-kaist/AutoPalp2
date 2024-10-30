using UnityEngine;
using TMPro;

public class TextPainScale : MonoBehaviour
{
    public TextMeshPro textMeshPro;
    public GameObject targetObject;
    private int currentValue = 5;

    private Vector3 initialPosition;

    private void Start()
    {
        textMeshPro.text = currentValue.ToString();
        initialPosition = targetObject.transform.position;
    }

    private void Update()
    {
        currentValue = Mathf.RoundToInt(65.78f * (targetObject.transform.position.x - initialPosition.x) + 5);
        currentValue = Mathf.Clamp(currentValue, 0, 10);
        textMeshPro.text = currentValue.ToString();

        return;
    }
}