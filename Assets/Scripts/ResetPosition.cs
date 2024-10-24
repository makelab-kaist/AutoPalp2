using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    public GameObject targetObject;
    public Vector3 targetPosition = new Vector3(-0.02f, 0.03450114f, 0.0007962286f);

    void Start()
    {
        targetObject.transform.position = targetPosition;
    }
}
