using System;
using UnityEngine;

public class SingleDoorScript : MonoBehaviour
{
    private GameObject doorL;
    private Vector3 doorLInitialPos;
    private Vector3 doorLPosEnd;

    public float doorMoveDuration = 2f;
    private float doorMoveProgress = 0f;
    private bool isOpening = false;

    public GameObject minigame1Object;
    private Minigame1 minigame1Script;

    private void Start()
    {
        doorL = GameObject.Find("SingleDoorL");
        doorLInitialPos = doorL.transform.position;
        doorLPosEnd = doorL.transform.position;
        doorLPosEnd.x += 2f;
        minigame1Script = minigame1Object.GetComponent<Minigame1>();

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

        // Lerp the door's position based on progress
        doorL.transform.position = Vector3.Lerp(doorLInitialPos, doorLPosEnd, doorMoveProgress);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is the player and Minigame 1 is won
        if (other.gameObject.CompareTag("Player") && minigame1Script.gameWon)
        {
            isOpening = true; // Start opening the door
        }
    }
}
