using UnityEngine;

public class PasswordPanel : MonoBehaviour
{
    public Renderer panelRenderer;
    public Material padRed;
    public Material padGreen;

    void Start()
    {
        if (panelRenderer != null && padRed != null)
        {
            panelRenderer.material = padRed;
        }
    }

    public void ChangeToGreen()
    {
        if (panelRenderer != null && padGreen != null)
        {
            panelRenderer.material = padGreen;
        }
    }
}
