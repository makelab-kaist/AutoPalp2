using UnityEngine;

/// <summary>
/// Toggles the active state of an array of GameObjects.
/// </summary>
public class GameObjectToggler : MonoBehaviour
{
    /// <summary>
    /// The array of target GameObjects whose active states will be toggled.
    /// </summary>
    [SerializeField]
    private GameObject[] targetObjects;

    /// <summary>
    /// Toggles the active state of all target GameObjects in the array.
    /// </summary>
    public void ToggleActiveStates()
    {
        if (targetObjects != null && targetObjects.Length > 0)
        {
            foreach (var obj in targetObjects)
            {
                if (obj != null)
                {
                    // Check the current active state and toggle it.
                    bool isGameObjectActive = obj.activeSelf;
                    obj.SetActive(!isGameObjectActive);
                }
                else
                {
                    Debug.LogWarning("One of the target objects is null. Please ensure all GameObjects in the array are assigned.");
                }
            }
        }
        else
        {
            Debug.LogWarning("No target objects assigned. Please assign GameObjects in the inspector.");
        }
    }
}