using UnityEngine;

public class OneThirdPosition : MonoBehaviour
{
    public GameObject objectA;
    public GameObject objectB;
    public GameObject markerPrefab;

    private GameObject markerInstance;

    void Start()
    {
        if (markerPrefab == null)
        {
            Debug.LogError("Marker Prefab is not assigned.");
            return;
        }

        markerInstance = Instantiate(markerPrefab);
    }

    void Update()
    {
        if (objectA != null && objectB != null && markerInstance != null)
        {
            Vector3 oneThirdPosition = Vector3.Lerp(objectA.transform.position, objectB.transform.position, 1.0f / 3.0f);

            markerInstance.transform.position = oneThirdPosition;
        }
    }
}
