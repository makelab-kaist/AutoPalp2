using UnityEngine;
using UnityEngine.UI;

public class UIScalerX : MonoBehaviour
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
            scaleSlider.value = targetObject.transform.localScale.x;

            // Add a listener to call the method when the slider value changes
            scaleSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    // Method to handle slider value changes
    public void OnSliderValueChanged(float value)
    {
        if (targetObject != null)
        {
            // Scale the object only on the X-axis
            Vector3 newScale = targetObject.transform.localScale;
            newScale.x = value;
            targetObject.transform.localScale = newScale;
        }
    }
}
