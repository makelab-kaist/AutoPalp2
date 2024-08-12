using UnityEngine;

public class OneThirdPosition : MonoBehaviour
{
    public GameObject objectA;
    public GameObject objectB;
    public GameObject markerPrefab;  // Reference to a prefab for the visual marker

    private GameObject markerInstance;  // To hold the instance of the marker

    void Start()
    {
        if (markerPrefab == null)
        {
            Debug.LogError("Marker Prefab is not assigned.");
            return;
        }

        // Instantiate the marker at the start (or you could instantiate it elsewhere)
        markerInstance = Instantiate(markerPrefab);
    }

    void Update()
    {
        if (objectA != null && objectB != null && markerInstance != null)
        {
            // Calculate the position that is 1/3 of the way between ObjectA and ObjectB
            Vector3 oneThirdPosition = Vector3.Lerp(objectA.transform.position, objectB.transform.position, 1.0f / 3.0f);

            // Update the marker's position
            markerInstance.transform.position = oneThirdPosition;
        }
    }
}
