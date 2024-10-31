using UnityEngine;

public class RestrictXAxis : MonoBehaviour
{
    void Update()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x = Mathf.Clamp(currentPosition.x, 0.29f, 0.45f);
        transform.position = currentPosition;
    }
}