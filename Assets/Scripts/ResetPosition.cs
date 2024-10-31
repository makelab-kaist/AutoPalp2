using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    public GameObject targetObject;
    public Vector3 targetPosition = new Vector3(0.3703214f, 1.059501f, 0.4200889f);

    void Start()
    {
        targetObject.transform.position = targetPosition;
    }
}
