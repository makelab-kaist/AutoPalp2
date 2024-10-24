using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeButton : MonoBehaviour
{
    public GameObject upperGameObject; // Assign in the inspector
    public GameObject lowerGameObject; // Assign in the inspector
    public GameObject objectToActivate; // The object to activate or deactivate

    private Vector3 initialPosition;
    public float pressDistanceThreshold = 0.1f; // The distance that qualifies as a press
    private bool isPressed = false;

    void Start()
    {
        // Store the initial position of the upper object
        initialPosition = upperGameObject.transform.position;
    }

    void Update()
    {
        // Measure the distance between the initial position and current position of the upper gameobject
        float distanceMoved = initialPosition.y - upperGameObject.transform.position.y;

        if (distanceMoved >= pressDistanceThreshold && !isPressed)
        {
            // Button pressed, trigger the event
            ActivateDeactivateObject();
            isPressed = true;
        }
        else if (distanceMoved < pressDistanceThreshold && isPressed)
        {
            // Reset button state when the cube is released
            isPressed = false;
        }
    }

    void ActivateDeactivateObject()
    {
        // Toggle the activation state of the target object
        if (objectToActivate != null)
        {
            bool currentState = objectToActivate.activeSelf;
            objectToActivate.SetActive(!currentState);
        }
    }
}
