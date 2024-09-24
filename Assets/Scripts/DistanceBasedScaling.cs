using UnityEngine;

public class DistanceBasedScaling : MonoBehaviour
{
    public GameObject sizeAdjustTop;
    public GameObject sizeAdjustLeft;
    public GameObject abdomen;

    private Vector3 initialScale;
    private float initialDistanceY;
    private float initialDistanceX;

    void Start()
    {
        if (abdomen != null)
        {
            initialScale = abdomen.transform.localScale;
            initialDistanceY = Vector3.Distance(sizeAdjustTop.transform.position, abdomen.transform.position);
            initialDistanceX = Vector3.Distance(sizeAdjustLeft.transform.position, abdomen.transform.position);
        }
    }

    void Update()
    {
        if (sizeAdjustTop != null && abdomen != null)
        {
            float updatedDistanceY = Vector3.Distance(sizeAdjustTop.transform.position, abdomen.transform.position);

            Vector3 newScale = initialScale;
            newScale.y = initialScale.y * updatedDistanceY / initialDistanceY;

            if (sizeAdjustLeft != null)
            {
                float updatedDistanceX = Vector3.Distance(sizeAdjustLeft.transform.position, abdomen.transform.position);
                newScale.x = initialScale.x * updatedDistanceX / initialDistanceX;
            }

            abdomen.transform.localScale = newScale;
        }
    }
}
