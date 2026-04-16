using TMPro;
using UnityEngine;

public class minigame3Trigger : MonoBehaviour
{
    CameraSwitcher camSwitch;
    public GameObject eventHandler;

    public TMP_Text minigameText; // Text displayed when player is in the trigger area
    Minigame3 mini3;
    public GameObject minigame3;

    public GameObject pc;
    PCScript pcScript;

    private bool stopLoop;
    private bool inTriggerArea = false; // Tracks if the player is in the trigger area
    public bool gameWon;
    private bool gameOver;

    public GameObject doorL;
    public GameObject doorR;

    private Vector3 doorLInitialPos;
    private Vector3 doorRInitialPos;

    private Vector3 doorLPosEnd;
    private Vector3 doorRPosEnd;

    public float doorMoveDuration = .8f; // Duration for the doors to fully open/close
    private float doorMoveProgress = 0f; // Tracks progress of the door movement
    private bool isOpening = false; // Tracks whether the doors are opening

    void Start()
    {
        camSwitch = eventHandler.GetComponent<CameraSwitcher>();
        mini3 = minigame3.GetComponent<Minigame3>();
        pcScript = pc.GetComponent<PCScript>();

        minigameText.gameObject.SetActive(false); // Hide text initially
        mini3.gameObject.SetActive(false);
        stopLoop = true;
        gameWon = false;

        doorLInitialPos = doorL.transform.position;
        doorRInitialPos = doorR.transform.position;

        doorLPosEnd = doorL.transform.position;
        doorLPosEnd.x += 2f;

        doorRPosEnd = doorR.transform.position;
        doorRPosEnd.x -= 2f;
    }

    void Update()
    {
        // Start the mini-game if conditions are met
        if (Input.GetKeyUp(KeyCode.K) && stopLoop && inTriggerArea && !gameWon && pcScript.KeycardCollected)
        {
            setTrue();
            stopLoop = false;
        }

        // Check if the mini-game is won
        if (mini3.gameWon && !gameWon)
        {
            Debug.Log("Mini-game won!");
            gameWon = true;
            mini3.gameWon = false; // Reset mini-game win status
            minigame3.SetActive(false); // Deactivate mini-game

            // If the player is in the trigger area, open the doors immediately
            if (inTriggerArea)
            {
                OpenDoors();
            }
        }

        // Update doors based on progress
        if (isOpening && doorMoveProgress < 1f)
        {
            doorMoveProgress += Time.deltaTime / doorMoveDuration;
            doorMoveProgress = Mathf.Clamp01(doorMoveProgress);
        }
        else if (!isOpening && doorMoveProgress > 0f)
        {
            doorMoveProgress -= Time.deltaTime / doorMoveDuration;
            doorMoveProgress = Mathf.Clamp01(doorMoveProgress);
        }

        // Lerp doors' positions based on progress
        doorL.transform.position = Vector3.Lerp(doorLInitialPos, doorLPosEnd, doorMoveProgress);
        doorR.transform.position = Vector3.Lerp(doorRInitialPos, doorRPosEnd, doorMoveProgress);
    }

    public void setTrue()
    {
        minigame3.SetActive(true);
        mini3.StartGame();
        minigameText.gameObject.SetActive(false); // Hide text when mini-game starts
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger area.");
            inTriggerArea = true;

            // Show "Press K" text only if the mini-game hasn't been won yet
            if (!gameWon && pcScript.KeycardCollected)
            {
                minigameText.gameObject.SetActive(true);
            }

            // Open doors if the game is won
            if (gameWon && gameOver == false)
            {
                OpenDoors();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited trigger area.");
            inTriggerArea = false;
            minigameText.gameObject.SetActive(false); // Hide text when player leaves trigger area

            // Close doors
            if (gameWon && gameOver == false)
            {
                Debug.Log("Closing doors.");
                isOpening = false;
                AudioManager.Instance.PlaySound("DoorClose");
                gameOver = true;
            }

        }
    }

    private void OpenDoors()
    {
        Debug.Log("Game won, opening doors!");
        isOpening = true;
        AudioManager.Instance.PlaySound("DoorOpen");
    }
}
