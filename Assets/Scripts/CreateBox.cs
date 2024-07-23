using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBox : MonoBehaviour
{
    public GameObject Panel;
    public GameObject Cube;

    public void OpenPanel()
    {
        if (Panel != null && Cube != null)
        {
            bool isPanelActive = Panel.activeSelf;
            bool isCubeActive = Cube.activeSelf;
            Panel.SetActive(!isPanelActive);
            Cube.SetActive(!isCubeActive);
        }
    }
}
