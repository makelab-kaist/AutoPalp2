using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates the displayed pain scale value based on the slider's current position.
/// </summary>
public class PainScaleDisplay : MonoBehaviour
{
    /// <summary>
    /// The slider component controlling the pain scale value.
    /// </summary>
    [SerializeField]
    private Slider painScaleSlider;

    /// <summary>
    /// The TextMeshProUGUI component displaying the current slider value.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI valueDisplayText;

    /// <summary>
    /// Updates the text display when the slider value changes.
    /// </summary>
    /// <param name="newValue">The new value of the slider.</param>
    public void UpdatePainScaleValue(float newValue)
    {
        valueDisplayText.text = $"{newValue}"; // Format to 1 decimal place for clarity.
    }
}