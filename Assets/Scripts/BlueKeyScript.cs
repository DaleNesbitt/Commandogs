using System.Collections;
using TMPro;
using UnityEngine;

public class BlueKeyScript : MonoBehaviour
{
    private bool inBlueKeyTriggerArea = false;
    private GameObject blueKeyLight;
    private TMP_Text blueKeyText;
    private GameObject blueKey;
    public bool blueKeyCollected = false;
    private TMP_Text keyPickupText;

    void Start()
    {
        blueKeyLight = GameObject.Find("BlueLight"); // Get and assign the blue light object (It's in a seperate parent)
        blueKey = GameObject.Find("BlueKey"); // Get and assign the visible blue key
        blueKeyText = GameObject.Find("BlueKeyText").GetComponent<TMP_Text>(); // Get the blue key text
        blueKeyText.gameObject.SetActive(false); // Make sure the text is false initially
        keyPickupText = GameObject.Find("KeyPickupText").GetComponent<TMP_Text>();
        keyPickupText.gameObject.SetActive(false);
    }

    void Update()
    {
        // If the player is in the trigger area and presses F, trigger the event
        if ((Input.GetKeyUp(KeyCode.X) || Input.GetButtonDown("Submit")) && inBlueKeyTriggerArea)
        {
            StartCoroutine(BlueKeyEvent());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Make sure it's only the player who can trigger the onTrigger
        if (other.CompareTag("Player") && !blueKeyCollected)
        {
            inBlueKeyTriggerArea = true;
            keyPickupText.gameObject.SetActive(true);
            keyPickupText.text = "Pickup Key\r\n(Press 'X/A')";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inBlueKeyTriggerArea = false;
            keyPickupText.gameObject.SetActive(false);
        }
    }

    IEnumerator BlueKeyEvent()
    {
        blueKeyCollected = true; // Tag the blue key as collected for other script
        keyPickupText.gameObject.SetActive(false);
        AudioManager.Instance.PlaySound("KeyPickup");
        blueKeyText.gameObject.SetActive(true); // Turn on the key collected tect
        blueKeyText.text = "Blue Key Collected!"; // Display text
        blueKeyLight.SetActive(false); // Turn off the pulse light
        blueKey.SetActive(false); // Turn off the visible key in the scene
        yield return new WaitForSeconds(3); // Wait for 3 seconds
        blueKeyText.gameObject.SetActive(false); // Hide the key collected text again
    }
}
