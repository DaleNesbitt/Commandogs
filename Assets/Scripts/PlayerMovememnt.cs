using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;
    private float rotationSpeed = 10f;
    private float speed;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Assign horizontal and vertical movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical);
        speed = movement.magnitude / 1.2f;

        if (speed > 0)
        {
            // Calculate the target rotation based on input direction
            Vector3 targetDirection = new Vector3(horizontal, 0, vertical).normalized;

            // Smoothly rotate towards the target direction
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        // Move the player in the forward direction
        Vector3 forwardMovement = transform.forward * speed;
        characterController.SimpleMove(forwardMovement * 5f);
    }
}
