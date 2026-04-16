using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.Cinemachine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public Transform rayOrigin;
    public float visionAngle = 90f;
    public float visionRange = 10f;
    public int rayCount = 20;

    public Transform[] points;
    private float walkSpeed = 1f;
    private NavMeshAgent agent;
    private int destPoint = 0;
    private bool isPaused = false;
    public bool playerIsDetected;

    [SerializeField] private GameObject alarmLight;
    private LevelManager levelManager;
    private AltPlayerMovement altPlayerMovement;
    private Animator animator;

    public CinemachineCamera enemyCamera;
    public CinemachineCamera defaultCamera;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.autoBraking = false;
        agent.speed = walkSpeed;
        levelManager = FindAnyObjectByType<LevelManager>();
        altPlayerMovement = FindAnyObjectByType<AltPlayerMovement>();

        GotoNextPoint();
    }


    private void GotoNextPoint()
    {
        // Return if no points are set
        if (points.Length == 0) 
                return; 

        // Set the next destination
        agent.destination = points[destPoint].position;

        // Increment the destination index
        destPoint = (destPoint + 1) % points.Length;
    }

    void Update()
    {
        // If the player is detected, stop all movement and exit Update early.
        if (playerIsDetected)
        {
            agent.isStopped = true; // Ensure the NavMeshAgent is stopped
            return;
        }

        // Check if the enemy reached their destination and isn't paused
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !isPaused)
        {
            StartCoroutine(PauseBeforeNextPoint());
        }

        // Check if the player is in the vision cone and not already detected
        if (IsPlayerInVisionCone() && !playerIsDetected)
        {
            SnapToEnemy(this.transform); // Snap to this instance of the enemy
        }
    }

    private IEnumerator PauseBeforeNextPoint()
    {
        isPaused = true; // Flag the enemy as paused
        agent.isStopped = true; // Stop the enemy's movement
        animator.SetInteger("isIdle", Random.Range(2,3)); // Set the animation to play the Idle anim
        yield return new WaitForSeconds(6.0f); // Wait 6 seconds
        agent.isStopped = false; // Continue player movement
        animator.SetInteger("isIdle", Random.Range(0,2)); // Set the animation to play one of 2 animations
        GotoNextPoint(); // Trigger the GoToNextPoint method
        isPaused = false; // Unflag the enemy
    }

    private bool IsPlayerInVisionCone()
    {
        // Calculate the direction vector from the enemy to the player
        Vector3 directionToPlayer = (player.position - rayOrigin.position).normalized;

        // Calculate the angle between the forward direction of the ray origin and the direction to the player
        float angleToPlayer = Vector3.Angle(rayOrigin.forward, directionToPlayer);

        // Check if the player is within the vision angle and range of the enemy
        if (angleToPlayer < visionAngle / 2 && Vector3.Distance(rayOrigin.position, player.position) <= visionRange)
        {
            // Cast multiple rays to simulate a vision cone
            for (int i = 0; i < rayCount; i++)
            {
                // Calculate the angle of each ray in the vision cone
                float angle = -visionAngle / 2 + (visionAngle / rayCount) * i;

                // Rotate the ray direction based on the angle
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
                Vector3 rayDirection = rotation * rayOrigin.forward;

                // Cast a ray from the ray origin in the calculated direction
                if (Physics.Raycast(rayOrigin.position, rayDirection, out RaycastHit hit, visionRange))
                {
                    // Check if the ray hits the player
                    if (hit.collider.CompareTag("Player"))
                    {
                        // Player is detected
                        return true;
                    }
                }
            }
        }

        // Player is not detected
        return false;
    }

    void OnDrawGizmos()
    {
        // If rayOrigin is not assigned, exit the method
        if (rayOrigin == null) return;

        // Set Gizmos color to blue for the vision cone
        Gizmos.color = Color.blue;

        // Draw rays to visualize the vision cone in the editor
        for (int i = 0; i < rayCount; i++)
        {
            // Calculate the angle of each ray in the vision cone
            float angle = -visionAngle / 2 + (visionAngle / rayCount) * i;

            // Rotate the ray direction based on the angle
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
            Vector3 rayDirection = rotation * rayOrigin.forward;

            // Draw the ray using Gizmos
            Gizmos.DrawRay(rayOrigin.position, rayDirection * visionRange);
        }
    }


    public void SnapToEnemy(Transform enemyTransform)
    {
        // Find the point where the camera will snap to. About 80% of the distance from the player TO the enemy
        Vector3 camSnapPosition = player.position + (enemyTransform.position - player.position) * 0.8f;

        // Set the camera's position to the midpoint but adjust Y to be at the player's eye level
        enemyCamera.transform.position = new Vector3(camSnapPosition.x, 1.5f, camSnapPosition.z);

        // Make the camera look at the enemy's position with a +2 offset on the Y-axis because the transform is on the enemy's feet
        Vector3 lookAtPosition = enemyTransform.position + new Vector3(0, 1.2f, 0);
        enemyCamera.transform.LookAt(lookAtPosition);

        // Increase the priority to switch to this camera. Cinemachine snaps to the highest priorty camera
        enemyCamera.Priority = 10;
        defaultCamera.Priority = 0;

        // Rotate the enemy to face the player by subtracting the players' vector from the transforms (enemy) and then normalising it.
        // Normailise means adjusting the vector so that its magnitude (length) becomes exactly 1, while keeping its direction the same.
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        PlayerDetected();
    }

    // Just incase I want to use it later
    public void RevertToDefault()
    {
        enemyCamera.Priority = 0;
        defaultCamera.Priority = 20;
    }

    public void PlayerDetected()
    {
        playerIsDetected = true;
        isPaused = true;
        agent.isStopped = true;
        animator.SetInteger("isIdle", 99); // Set the animation to play the player detected animation
        AudioManager.Instance.PlaySound("Alarm"); // Trigger the alarm
        AudioManager.Instance.PlaySound("Intruder"); // Trigger the intruder sound
        alarmLight.SetActive(true); // Turn on the light
        altPlayerMovement.runStopped = false;

        StartCoroutine(WaitAndReturnToMenu());
    }

    private IEnumerator WaitAndReturnToMenu()
    {
        yield return new WaitForSeconds(3.5f);
        levelManager.MainMenu();
    }

}
