using UnityEngine;

public class RestrictXAxis : MonoBehaviour
{
    void Update()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x = Mathf.Clamp(currentPosition.x, 0.294f, 0.446f);
        transform.position = currentPosition;
    }
}