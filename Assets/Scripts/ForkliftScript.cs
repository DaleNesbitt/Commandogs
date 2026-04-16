using System.Collections;
using TMPro;
using UnityEngine;

public class ForkliftScript : MonoBehaviour
{
    private bool inForkliftKeyTriggerArea = false;
    private GameObject forkliftKeyLight;
    private TMP_Text forkliftKeyText;
    private GameObject forkliftKey;
    private GameObject forkLiftPhys;
    public bool forkliftKeyCollected = false;
    private TMP_Text forkliftkeyPickupText;

    void Start()
    {
        forkliftKeyLight = GameObject.Find("ForkLight"); // Get and assign the blue light object (It's in a seperate parent)
        forkliftKey = GameObject.Find("ForkliftKey"); // Get and assign the visible blue key
        forkLiftPhys = GameObject.Find("ForkliftPhys"); // Get and assign the visible blue key
        forkliftKeyText = GameObject.Find("ForkliftKeyText").GetComponent<TMP_Text>(); // Get the forklift key text
        forkliftKeyText.gameObject.SetActive(false); // Make sure the text is false initially
        forkliftkeyPickupText = GameObject.Find("ForkliftkeyPickupText").GetComponent<TMP_Text>();
        forkliftkeyPickupText.gameObject.SetActive(false);
    }

    void Update()
    {
        // If the player is in the trigger area and presses F, trigger the event
        if ((Input.GetKeyUp(KeyCode.X) || Input.GetButtonDown("Submit")) && inForkliftKeyTriggerArea)        {
            StartCoroutine(ForkliftKeyEvent());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Make sure it's only the player who can trigger the onTrigger
        if (other.CompareTag("Player") && !forkliftKeyCollected)
        {
            inForkliftKeyTriggerArea = true;
            forkliftkeyPickupText.gameObject.SetActive(true);
            forkliftkeyPickupText.text = "Pickup Key\r\n(Press 'X/A')";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inForkliftKeyTriggerArea = false;
            forkliftkeyPickupText.gameObject.SetActive(false);
        }
    }

    IEnumerator ForkliftKeyEvent()
    {
        forkliftKeyCollected = true; // Tag the blue key as collected for other script
        forkliftkeyPickupText.gameObject.SetActive(false);
        AudioManager.Instance.PlaySound("KeyPickup");
        forkliftKeyText.gameObject.SetActive(true); // Turn on the key collected tect
        forkliftKeyText.text = "Forklift Key Collected!"; // Display text
        forkliftKeyLight.SetActive(false); // Turn off the pulse light
        forkLiftPhys.SetActive(false); // Turn off the visible key in the scene
        yield return new WaitForSeconds(3); // Wait for 3 seconds
        forkliftKeyText.gameObject.SetActive(false); // Hide the key collected text again
        forkliftKey.SetActive(false); // Turn off the visible key in the scene
    }
}
