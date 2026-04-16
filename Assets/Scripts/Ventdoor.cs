using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class Ventdoor : MonoBehaviour
{
    private Animator animator;
    private CageKeyPadScript cageKeyPadScript;
    [SerializeField] private CinemachineCamera defaultCam;
    [SerializeField] private CinemachineCamera prisonCam;


    // Reference to the PrisonAreaMovement script
    private Dogs dogScript;

    void Start()
    {
        animator = GetComponent<Animator>();
        cageKeyPadScript = Object.FindFirstObjectByType<CageKeyPadScript>();

        // Find the parent object that holds all the dogs
        GameObject prisonArea = GameObject.Find("PrisonArea");
        if (prisonArea != null)
        {
            dogScript = prisonArea.GetComponent<Dogs>();
        }
        else
        {
            Debug.LogError("PrisonArea not found in scene.");
        }
    }

    void Update()
    {
        if (cageKeyPadScript.codeCorrect == true)
        {
            StartCoroutine(OpenVent());
        }
    }

    IEnumerator OpenVent()
    {
        if (animator != null)
        {
            animator.SetBool("codeCorrect", true);
        }

        // Set the prisonCam's priority high so that it takes over
        prisonCam.Priority = 100;

        // Start the escape sequence for the dogs
        if (dogScript != null)
        {
            dogScript.TriggerEscape();
        }

        // Wait for 10 seconds before returning the camera
        yield return new WaitForSeconds(5);

        // Return prisonCam to its normal (lower) priority so the default camera resumes control
        prisonCam.Priority = 0;
        prisonCam.gameObject.SetActive(false); // Optionally disable the prisonCam after use
    }
}
