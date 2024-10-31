using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PainScaleValue : MonoBehaviour
{
    [SerializeField]
    Slider slider;

    [SerializeField]
    TextMeshProUGUI text;

    public void OnSliderEvent(float volume)
    {
        text.text = $"{volume}";
    }
}
