using UnityEngine;

public class DistanceBasedScaling : MonoBehaviour
{
    public GameObject sizeAdjustTop;
    public GameObject sizeAdjustLeft;
    public GameObject abdomen;

    public Vector3 initialScale;

    public float scaleFactorY = 1.0f;
    public float scaleFactorX = 1.0f;
    public float minScaleMultiplier = 0.5f;
    public float maxScaleMultiplier = 2.0f;
    public float distanceFactor = 0.6f;
    public float scaleFactor = 1.5f;

    void Start()
    {
        if (abdomen != null)
        {
            initialScale = abdomen.transform.localScale;
        }

        Collider colliderTop = sizeAdjustTop.GetComponent<Collider>();
        Collider colliderLeft = sizeAdjustLeft.GetComponent<Collider>();
        Collider colliderAbdomen = abdomen.GetComponent<Collider>();

        if (colliderTop != null && colliderLeft != null && colliderAbdomen != null)
        {
            Physics.IgnoreCollision(colliderTop, colliderAbdomen);
            Physics.IgnoreCollision(colliderLeft, colliderAbdomen);
        }
    }

    void Update()
    {
        if (sizeAdjustTop != null && abdomen != null)
        {
            float distanceY = distanceFactor * Vector3.Distance(sizeAdjustTop.transform.position, abdomen.transform.position);
            float scaleMultiplierY = Mathf.Clamp(minScaleMultiplier + (distanceY * scaleFactorY), minScaleMultiplier, maxScaleMultiplier);

            Vector3 newScale = initialScale;
            newScale.y = initialScale.y * scaleMultiplierY;

            if (sizeAdjustLeft != null)
            {
                float distanceX = distanceFactor * Vector3.Distance(sizeAdjustLeft.transform.position, abdomen.transform.position);
                float scaleMultiplierX = Mathf.Clamp(minScaleMultiplier + (distanceX * scaleFactorX), minScaleMultiplier, maxScaleMultiplier);

                newScale.x = initialScale.x * scaleMultiplierX;
            }

            abdomen.transform.localScale = scaleFactor * newScale;
        }
    }
}
