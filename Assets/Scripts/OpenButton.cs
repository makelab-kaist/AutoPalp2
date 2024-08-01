using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenButton : MonoBehaviour
{
    public GameObject Object;

    public void OpenObject()
    {
        if (Object != null)
        {
            Object.SetActive(true);
        }
    }
}
