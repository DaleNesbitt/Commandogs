using UnityEngine;
using Unity.Cinemachine;

public class VentScript : MonoBehaviour
{
    public CinemachineCamera firstPersonCamera;
    public CinemachineCamera defaultCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SnapToFirstPerson();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            RevertToDefault();
        }
    }

    public void SnapToFirstPerson()
    {
        firstPersonCamera.Priority = 10;
        defaultCamera.Priority = 5;
    }

    public void RevertToDefault()
    {
        firstPersonCamera.Priority = 5;
        defaultCamera.Priority = 10;
    }
}
