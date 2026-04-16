using UnityEngine;

public class AttachCrateToArm : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Crate_Floppy"))
        {
            // Set the crate as a child of this object (the Arm)
            other.transform.SetParent(transform);
        }
    }
}
