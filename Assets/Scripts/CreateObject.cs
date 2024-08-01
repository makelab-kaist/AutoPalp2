using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour
{
    public GameObject Object;

    public void OpenObject()
    {
        if (Object != null)
        {
            bool isCubeActive = Object.activeSelf;
            Object.SetActive(!isCubeActive);
        }
    }
}
