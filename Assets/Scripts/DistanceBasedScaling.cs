using UnityEngine;

public class DistanceBasedScaling : MonoBehaviour
{
    public GameObject abdomen;
    public GameObject sizeAdjustLeft;
    public GameObject sizeAdjustTop;

    private Vector3 initialScale;
    private float initialDistanceX;
    private float initialDistanceY;

    void Start()
    {
        initialScale = abdomen.transform.localScale;
        initialDistanceX = Vector3.Distance(sizeAdjustLeft.transform.position, abdomen.transform.position);
        initialDistanceY = Vector3.Distance(sizeAdjustTop.transform.position, abdomen.transform.position);
    }

    void Update()
    {
        Vector3 newScale = initialScale;

        float updatedDistanceX = Vector3.Distance(sizeAdjustLeft.transform.position, abdomen.transform.position);
        float updatedDistanceY = Vector3.Distance(sizeAdjustTop.transform.position, abdomen.transform.position);

        newScale.x = initialScale.x * updatedDistanceX / initialDistanceX;
        newScale.y = initialScale.y * updatedDistanceY / initialDistanceY;

        abdomen.transform.localScale = newScale;
    }
}
