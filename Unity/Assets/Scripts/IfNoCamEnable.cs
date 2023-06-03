using UnityEngine;

public class IfNoCamEnable : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnEnable()
    {
        if (!Camera.main)
            gameObject.tag = "MainCamera";
    }
    private void Update()
    {
        if (Camera.main && Camera.main.transform != transform)
            gameObject.SetActive(false);
    }

#endif
}
