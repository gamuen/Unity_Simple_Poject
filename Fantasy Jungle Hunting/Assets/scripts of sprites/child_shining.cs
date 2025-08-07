using UnityEngine;

public class AttachMultipleChildren : MonoBehaviour
{
    public GameObject parentObject;
    public GameObject[] childObjects;

    void Start()
    {
        foreach (GameObject child in childObjects)
        {
            if (child != null)
                child.transform.SetParent(parentObject.transform, false);
        }
    }
}
