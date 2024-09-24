using UnityEngine;

public class DistanceBasedScaling : MonoBehaviour
{
    public GameObject sizeAdjustTop;
    public GameObject sizeAdjustLeft;
    public GameObject abdomen;

    private Vector3 initialScale;
    private float initialDistanceY;
    private float initialDistanceX;

    // public float scaleFactorY = 1.0f;
    // public float scaleFactorX = 1.0f;
    // public float minScaleMultiplier = 0.5f;
    // public float maxScaleMultiplier = 2.0f;
    // public float distanceFactor = 0.6f;
    // public float scaleFactor = 1.5f;

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

            // float distanceY = distanceFactor * Vector3.Distance(sizeAdjustTop.transform.position, abdomen.transform.position);
            // float scaleMultiplierY = Mathf.Clamp(minScaleMultiplier + (distanceY * scaleFactorY), minScaleMultiplier, maxScaleMultiplier);

            Vector3 newScale = initialScale;
            newScale.y = initialScale.y * updatedDistanceY / initialDistanceY;

            if (sizeAdjustLeft != null)
            {
                float updatedDistanceX = Vector3.Distance(sizeAdjustLeft.transform.position, abdomen.transform.position);
                newScale.x = initialScale.x * updatedDistanceX / initialDistanceX;


                //     float distanceX = distanceFactor * Vector3.Distance(sizeAdjustLeft.transform.position, abdomen.transform.position);
                //     float scaleMultiplierX = Mathf.Clamp(minScaleMultiplier + (distanceX * scaleFactorX), minScaleMultiplier, maxScaleMultiplier);

                //     newScale.x = initialScale.x * scaleMultiplierX;
            }

            abdomen.transform.localScale = newScale;
        }
    }
}
