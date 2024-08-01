using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAll : MonoBehaviour
{
    public GameObject Interactables;
    public GameObject AdvancedPoseDebugger;

    public void CloseAllObject()
    {
        if (Interactables != null && AdvancedPoseDebugger != null)
        {
            Interactables.SetActive(false);
            AdvancedPoseDebugger.SetActive(false);
        }
    }
}
