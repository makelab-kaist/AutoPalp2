using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Resets the slider value to a default value when the script starts.
/// </summary>
public class SliderValueResetter : MonoBehaviour
{
    /// <summary>
    /// The slider whose value will be reset.
    /// </summary>
    [SerializeField]
    private Slider targetSlider;

    /// <summary>
    /// The default value to reset the slider to.
    /// </summary>
    [SerializeField]
    private float defaultValue = 5f;

    void Start()
    {
        // Ensure the slider reference is not null.
        if (targetSlider != null)
        {
            // Set the slider's value to the default value.
            targetSlider.value = defaultValue;
        }
        else
        {
            Debug.LogWarning("Slider reference is missing. Please assign a slider in the inspector.");
        }
    }
}
