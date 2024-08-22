using UnityEngine;

public class DistanceBasedScaling : MonoBehaviour
{
    public GameObject objectA;  // Y축 크기를 조절하는 오브젝트
    public GameObject objectB;  // 크기가 변하는 오브젝트
    public GameObject objectC;  // X축 크기를 조절하는 새로운 오브젝트

    public float scaleFactorY = 1.0f;  // Y축 거리 비례 계수
    public float scaleFactorX = 1.0f;  // X축 거리 비례 계수
    public Vector3 initialScale;  // 초기 크기
    public float minScaleMultiplier = 0.5f;  // 최소 크기 배수
    public float maxScaleMultiplier = 2.0f;  // 최대 크기 배수

    void Start()
    {
        if (objectB != null)
        {
            initialScale = objectB.transform.localScale;  // 오브젝트 B의 초기 크기 저장
        }

        Collider colliderA = objectA.GetComponent<Collider>();
        Collider colliderB = objectB.GetComponent<Collider>();

        if (colliderA != null && colliderB != null)
        {
            Physics.IgnoreCollision(colliderA, colliderB);
        }
    }

    void Update()
    {
        if (objectA != null && objectB != null)
        {
            // objectA와 objectB 사이의 거리 계산 (Y축 크기 조절)
            float distanceY = Vector3.Distance(objectA.transform.position, objectB.transform.position);
            float scaleMultiplierY = Mathf.Clamp(minScaleMultiplier + (distanceY * scaleFactorY), minScaleMultiplier, maxScaleMultiplier);

            // Y축 크기 조정
            Vector3 newScale = initialScale;
            newScale.y = initialScale.y * scaleMultiplierY;

            // 만약 objectC가 설정되어 있다면, X축 크기도 조절
            if (objectC != null)
            {
                // objectC와 objectB 사이의 거리 계산 (X축 크기 조절)
                float distanceX = Vector3.Distance(objectC.transform.position, objectB.transform.position);
                float scaleMultiplierX = Mathf.Clamp(minScaleMultiplier + (distanceX * scaleFactorX), minScaleMultiplier, maxScaleMultiplier);

                // X축 크기 조정
                newScale.x = initialScale.x * scaleMultiplierX;
            }

            // 오브젝트 B의 크기 설정
            objectB.transform.localScale = newScale;
        }
    }
}
