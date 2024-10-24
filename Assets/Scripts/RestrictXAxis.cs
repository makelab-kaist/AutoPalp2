using UnityEngine;

public class RestrictXAxis : MonoBehaviour
{
    void Update()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x = Mathf.Clamp(currentPosition.x, -0.12f, 0.08f);
        transform.position = currentPosition;
    }
}