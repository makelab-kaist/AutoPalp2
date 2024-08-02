using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueUpdater : MonoBehaviour
{
    public Slider SliderThing;
    public GameObject Text;

    void Start()
    {

    }

    void Update()
    {
        Text.GetComponent<TextMeshPro>().text = SliderThing.value.ToString();
    }
}
