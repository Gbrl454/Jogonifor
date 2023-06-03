using UnityEngine;

public class Mesh2Collider : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] GameObject source;

    [ContextMenu("Run")]
    void ProcessAll()
    {
        Renderer[] renderers = source == null ? GetComponentsInChildren<Renderer>()
            : source.GetComponentsInChildren<Renderer>();

        foreach (var item in renderers)
        {
            GameObject go = item.gameObject;

            if (!go.GetComponent<BoxCollider>())
                go.AddComponent<BoxCollider>();

            DestroyImmediate(GetComponent<MeshFilter>());
            DestroyImmediate(item);
        }
    }
#endif
}
