using System.Collections;
using UnityEngine;

public class CubeHighlighter : MonoBehaviour
{
    public GameObject[] cubes;
    public Material newMaterial;
    private Material originalMaterial;

    private void OnEnable()
    {
        originalMaterial = cubes[0].GetComponent<Renderer>().material;
        StartCoroutine(ChangeMaterialsSequentially());
    }

    private IEnumerator ChangeMaterialsSequentially()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].GetComponent<Renderer>().material = newMaterial;

            yield return new WaitForSeconds(0.15f);

            cubes[i].GetComponent<Renderer>().material = originalMaterial;

            yield return new WaitForSeconds(0.15f);
        }
    }
}