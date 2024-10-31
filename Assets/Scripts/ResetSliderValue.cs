using UnityEngine;
using UnityEngine.UI;

public class ResetSliderValue : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        if (slider != null)
        {
            slider.value = 5;
        }
    }
}
