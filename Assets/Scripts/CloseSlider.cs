using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseSlider : MonoBehaviour
{
    public GameObject slider;

    public void CloseSliderObject()
    {
        if (slider != null)
        {
            slider.SetActive(false);
        }
    }
}
