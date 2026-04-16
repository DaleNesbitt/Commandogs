using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class GreenTableScript : MonoBehaviour
{
    private GameObject blueKey;
    private GameObject arm;
    private BlueKeyScript blueKeyScript;
    private bool inGreenTableTriggerArea = false;
    private Animator armAnimator;
    private TMP_Text blueAreaText;
    private TMP_Text operateArmText;
    private bool armTriggered = false;
    private AudioSource armAudioSource;
    [SerializeField]
    private AudioClip armSound;
    private bool soundPlayed;


    void Start()
    {
        blueKey = GameObject.Find("Blue_Key");
        blueKeyScript = blueKey.gameObject.GetComponent<BlueKeyScript>();

        arm = GameObject.Find("Arm_1");
        armAnimator = arm.GetComponent<Animator>();
        armAudioSource = arm.GetComponent<AudioSource>();

        blueAreaText = GameObject.Find("BlueAreaText").GetComponent<TMP_Text>();
        blueAreaText.gameObject.SetActive(false);

        operateArmText = GameObject.Find("OperateArmText").GetComponent<TMP_Text>();
        operateArmText.gameObject.SetActive(false);

    }

    void Update()
    {
        // If the player is in the trigger area, has the blue key, and presses X/A, trigger the event
        if (((Input.GetKeyUp(KeyCode.X) || Input.GetButtonDown("Submit")) && inGreenTableTriggerArea && blueKeyScript.blueKeyCollected && !soundPlayed))
        {
            OpenCrateEvent();
        }
    }


    void OnTriggerEnter(Collider other)
    {
        // Make sure it's only the player who can trigger the onTrigger
        if (other.CompareTag("Player"))
        {
            inGreenTableTriggerArea = true;

            if (!blueKeyScript.blueKeyCollected)
            {
                blueAreaText.gameObject.SetActive(true); // Turn on the key collected tect
                blueAreaText.text = "Key required to operate mechanical arm"; // Display text
            }
            else if (blueKeyScript.blueKeyCollected && !armTriggered)
            {
                operateArmText.gameObject.SetActive(true); // Turn on the key collected tect
                operateArmText.text = "Operate Arm\r\n(Press 'X/A')";
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inGreenTableTriggerArea = false;
            blueAreaText.gameObject.SetActive(false);
            operateArmText.gameObject.SetActive(false);
        }
    }

    public void OpenCrateEvent()
    {
        soundPlayed = true;
        armAudioSource.PlayOneShot(armSound);
        armTriggered = true;
        operateArmText.gameObject.SetActive(false);
        armAnimator.SetBool("ArmTriggered", true);
    }
}
