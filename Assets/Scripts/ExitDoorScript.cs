using System;
using UnityEngine;

public class ExitDoorScript : MonoBehaviour
{
    private GameObject doorL;
    private GameObject doorR;

    private Vector3 doorLInitialPos;
    private Vector3 doorRInitialPos;

    private Vector3 doorLPosEnd;
    private Vector3 doorRPosEnd;

    public float doorMoveDuration = 2f;
    private float doorMoveProgress = 0f;
    private bool isOpening = false;
    private bool gameOver;

    public GameObject mini3;
    private Minigame3 mini3Script;

    private void Start()
    {
        doorL = GameObject.Find("ExitDoorL");
        doorR = GameObject.Find("ExitDoorR");

        doorLInitialPos = doorL.transform.position;
        doorRInitialPos = doorR.transform.position;

        doorLPosEnd = doorL.transform.position;
        doorLPosEnd.x += 2f;

        doorRPosEnd = doorR.transform.position;
        doorRPosEnd.x -= 2f;

        mini3Script = mini3.GetComponent<Minigame3>();
    }

    private void Update()
    {
        if (mini3Script != null && mini3Script.gameWon)
        {
            // Open doors if the minigame is won
            if (isOpening && doorMoveProgress < 1f)
            {
                doorMoveProgress += Time.deltaTime / doorMoveDuration;
                doorMoveProgress = Mathf.Clamp01(doorMoveProgress);
            }

            doorL.transform.position = Vector3.Lerp(doorLInitialPos, doorLPosEnd, doorMoveProgress);
            doorR.transform.position = Vector3.Lerp(doorRInitialPos, doorRPosEnd, doorMoveProgress);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && gameOver == false)
        {
            isOpening = true; // Start opening doors when player enters trigger
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameOver = true;
            isOpening = false;
        }
    }
}
