using UnityEngine;
using UnityEngine.UI;

public class UIScalerZ : MonoBehaviour
{
    // Reference to the slider
    public Slider scaleSlider;

    // Reference to the object you want to scale
    public GameObject targetObject;

    // Minimum and maximum scale values
    public float minScale = 0.5f;
    public float maxScale = 2.0f;

    void Start()
    {
        // Initialize the slider value
        if (scaleSlider != null)
        {
            scaleSlider.minValue = minScale;
            scaleSlider.maxValue = maxScale;
            scaleSlider.value = targetObject.transform.localScale.z;

            // Add a listener to call the method when the slider value changes
            scaleSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    // Method to handle slider value changes
    public void OnSliderValueChanged(float value)
    {
        if (targetObject != null)
        {
            // Scale the object only on the Y-axis
            Vector3 newScale = targetObject.transform.localScale;
            newScale.z = value;
            targetObject.transform.localScale = newScale;
        }
    }
}
