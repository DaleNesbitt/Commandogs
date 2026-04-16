using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AltPlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;
    private float speed;
    private AudioSource audioSource;
    private float defaultPitch;
    public bool runStopped;
    private CinemachineSplineDolly dollyCam;

    // Reference to the PrisonCell script
    public PrisonCell prisonCellScript;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        defaultPitch = audioSource.pitch = .65f; // Set the pitch at 65%
        dollyCam = FindFirstObjectByType<CinemachineSplineDolly>();
        if (dollyCam == null)
        {
            Debug.LogWarning("CinemachineSplineDolly not found in the scene.");
        }

        // Use Resources.FindObjectsOfTypeAll to find inactive objects
        PrisonCell[] prisonCells = Resources.FindObjectsOfTypeAll<PrisonCell>();
        if (prisonCells.Length > 0)
        {
            prisonCellScript = prisonCells[0];
            //Debug.Log("PrisonCell script found (even though its GameObject is inactive).");
        }
        else
        {
            Debug.LogWarning("PrisonCell script not found in the scene.");
        }
    }

    void Update()
    {
        // Prevent movement when the keypad UI is active
        if (prisonCellScript != null && prisonCellScript.keypadUI.activeSelf)
        {
            return;  // Early exit to prevent movement if keypad is active
        }

        IntroPlaying();

        // Assign horizontal and vertical movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical);
        speed = movement.magnitude;

        // Ensure movement is fully stopped when runStopped is true
        if (runStopped)
        {
            characterController.SimpleMove(Vector3.zero);
            animator.SetBool("isRunning", false);
            audioSource.pitch = defaultPitch;
            return;
        }

        if (speed > 0)
        {
            transform.Rotate(Vector3.up, horizontal * 180f * Time.deltaTime, Space.Self);
            animator.SetBool("isRunning", true);

            if (movement.z >= 0.1)
            {
                audioSource.pitch = 1.0f;
            }
            movement = transform.forward * vertical;
            characterController.SimpleMove(movement * 3.1f);
        }
        else
        {
            animator.SetBool("isRunning", false);
            audioSource.pitch = defaultPitch;
        }
    }



    void IntroPlaying()
    {
        if (dollyCam != null && dollyCam.CameraPosition != 1)
        {
            runStopped = true;
        }
        else
        {
            runStopped = false;
            if (dollyCam != null)
            {
                dollyCam.gameObject.SetActive(false);
            }
        }
    }
}
