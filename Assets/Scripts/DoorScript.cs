using System;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject doorL;
    public GameObject doorR;

    private Vector3 doorLInitialPos;
    private Vector3 doorRInitialPos;

    private Vector3 doorLPosEnd;
    private Vector3 doorRPosEnd;

    public float doorMoveDuration = 0.8f; // Duration for the doors to fully open/close
    private float doorMoveProgress = 0f; // Tracks progress of the door movement
    private bool isOpening = false; // Tracks whether the doors are opening or closing
    private bool welcomePlayed = false;

    private void Start()
    {
        PlayIntro();
        // Get the initial pos of the the doors
        doorLInitialPos = doorL.transform.position;
        doorRInitialPos = doorR.transform.position;

        // Get the position of where the doors will stop opening
        doorLPosEnd = doorL.transform.position;
        doorLPosEnd.x += 2f;
        doorRPosEnd = doorR.transform.position;
        doorRPosEnd.x -= 2f;
    }

    private void Update()
    {
        if (isOpening && doorMoveProgress < 1f)
        {
            // Increment progress for opening
            doorMoveProgress += Time.deltaTime / doorMoveDuration;
            doorMoveProgress = Mathf.Clamp01(doorMoveProgress);
        }
        else if (!isOpening && doorMoveProgress > 0f)
        {
            // Decrement progress for closing
            doorMoveProgress -= Time.deltaTime / doorMoveDuration;
            doorMoveProgress = Mathf.Clamp01(doorMoveProgress);
        }

        // Lerp doors' positions based on progress
        doorL.transform.position = Vector3.Lerp(doorLInitialPos, doorLPosEnd, doorMoveProgress);
        doorR.transform.position = Vector3.Lerp(doorRInitialPos, doorRPosEnd, doorMoveProgress);
    }

    private void OnTriggerEnter(Collider other)
    {
        // When the player walks intot he collider
        if (other.gameObject.CompareTag("Player"))
        {
            // Play the door open SFX
            AudioManager.Instance.PlaySound("DoorOpen");
            isOpening = true; // Start opening doors
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Play doors closing sound
            AudioManager.Instance.PlaySound("DoorClose");
            isOpening = false; // Start closing doors
        }
    }

    private void PlayIntro()
    {
        // If the welcome audio hasn't been played already
        if (!welcomePlayed)
        {
            // Generate a random number and add it to the end of the sound name
            int randomSoundNum = UnityEngine.Random.Range(1, 11);

            // For  example, there are 10 'Welcome' audio clips. This will choose one based on the random number. E.g "Welcome(4)"
            AudioManager.Instance.PlaySound("Welcome" + randomSoundNum.ToString());
            welcomePlayed = true;
        }
    }
}
