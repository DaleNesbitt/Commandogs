using TMPro;
using UnityEngine;

public class minigameTrigger : MonoBehaviour
{
    // Assign variables
    public PasswordPanel passwordPanel;

    public TMP_Text minigameText;
    public GameObject minigame1;
    public GameObject doorL;
    public GameObject eventHandler;

    private CameraSwitcher camSwitch;
    private Minigame1 mini1;
    private bool inTriggerArea;
    private bool accessGrantedPlayed = false;

    private Vector3 doorLInitialPos;
    private Vector3 doorLPosEnd;
    private float doorMoveProgress = 0f;
    public float doorMoveDuration = 2f;

    void Start()
    {
        // Assign components
        camSwitch = eventHandler.GetComponent<CameraSwitcher>();
        mini1 = minigame1.GetComponent<Minigame1>();

        // Initialize door positions
        doorLInitialPos = doorL.transform.position;
        doorLPosEnd = doorL.transform.position + new Vector3(2f, 0, 0);

        // Setup UI
        minigameText.gameObject.SetActive(false);
        mini1.gameObject.SetActive(false);
        inTriggerArea = false;
    }

    void Update()
    {
        // If player 1 is in the trigger area and minigame 1 isn't won and player 2 presses L, trigger method
        if (Input.GetKeyUp(KeyCode.L) && inTriggerArea && !mini1.gameWon)
        {
            setTrue();
        }

        if (mini1.gameWon && doorMoveProgress < 1f)
        {
            // Increment progress for opening
            doorMoveProgress += Time.deltaTime / doorMoveDuration;
            doorMoveProgress = Mathf.Clamp01(doorMoveProgress);

            // Open the door
            doorL.transform.position = Vector3.Lerp(doorLInitialPos, doorLPosEnd, doorMoveProgress);

            if (!accessGrantedPlayed)
            {
                // Play audio and change the panel colour
                AudioManager.Instance.PlaySound("DoorOpen");
                passwordPanel.ChangeToGreen();
                // Generate a random number and add it to the end of the sound name
                int randomSoundNum = UnityEngine.Random.Range(1, 8);
                AudioManager.Instance.PlaySound("AccessGranted" + randomSoundNum.ToString());
                accessGrantedPlayed = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !mini1.gameWon)
        {
            minigameText.gameObject.SetActive(true);
            inTriggerArea = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            minigameText.gameObject.SetActive(false);
            inTriggerArea = false;
        }
    }

    public void setTrue()
    {
        minigame1.gameObject.SetActive(true);
        mini1.GameStart();
        minigameText.gameObject.SetActive(false);
    }
}
