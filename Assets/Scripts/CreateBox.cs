using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBox : MonoBehaviour
{
    public GameObject Cube;

    public void OpenPanel()
    {
        if (Cube != null)
        {
            bool isCubeActive = Cube.activeSelf;
            Cube.SetActive(!isCubeActive);
        }
    }
}
