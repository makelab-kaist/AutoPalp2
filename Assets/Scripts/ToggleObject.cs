using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public GameObject Object;

    public void OpenObject()
    {
        if (Object != null)
        {
            bool isGameObjectActive = Object.activeSelf;
            Object.SetActive(!isGameObjectActive);
        }
    }
}
