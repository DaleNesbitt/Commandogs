using UnityEngine;

public class Laser : MonoBehaviour
{
    public Transform laserOrigin;
    public LineRenderer lineRenderer;
   // private bool isOn = true;

    void Update()
    {
        Vector3 startPoint = laserOrigin.position;
        Vector3 endPoint = laserOrigin.position + laserOrigin.forward ;

        // Check for collisions
        if (Physics.Raycast(laserOrigin.position, laserOrigin.forward, out RaycastHit hit))
        {
            endPoint = hit.point;
        }

        // Set the Line Renderer positions
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }
}
