using UnityEngine;

// Forces a component. If the object this script is attached to doesn't have a component, this will add one. A Renderer in this case
[RequireComponent(typeof(Renderer))]
public class TextureTilingScript : MonoBehaviour
{
    void Start()
    {
        // Get the Renderer component of the object
        Renderer renderer = GetComponent<Renderer>();

        // Get the scale of the object (wall or whatever)
        Vector3 scale = transform.localScale;

        // Adjust the material's texture tiling to match the object's scale
        if (renderer.material != null)
        {
            // So instead of setting the tiling manually, this sets the X and Y of the tiling to the numbers got from the scale of the object
            renderer.material.mainTextureScale = new Vector2(scale.z / 7.17f, scale.y / 6);
        }
    }
}
