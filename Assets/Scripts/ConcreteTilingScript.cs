using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ConcreteTilingScript : MonoBehaviour
{
    void Start()
    {
        // Get the Renderer component of the object
        Renderer renderer = GetComponent<Renderer>();

        // Get the scale of the object
        Vector3 scale = transform.localScale;

        // Adjust the material's texture tiling to match the object's scale
        if (renderer.material != null)
        {
            renderer.material.mainTextureScale = new Vector2(scale.z / 8.4f, scale.y / 6);
        }
    }
}
