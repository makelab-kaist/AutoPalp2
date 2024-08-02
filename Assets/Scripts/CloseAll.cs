using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAll : MonoBehaviour
{
    public GameObject Interactables;
    public GameObject AdvancedPoseDebugger;
    public GameObject PatientInfo;

    public void CloseAllObject()
    {
        if (Interactables != null
            && AdvancedPoseDebugger != null
            && PatientInfo != null)
        {
            Interactables.SetActive(false);
            AdvancedPoseDebugger.SetActive(false);
            PatientInfo.SetActive(false);
        }
    }
}
