using TMPro;
using UnityEngine;

public class CageKeyPadScript : MonoBehaviour
{
    private EnemyMovement enemyMovementScript;
    private bool inCageKeypadArea = false; // Is the player in the keypad area?
    public bool codeCorrect = false;       // Tracks if the correct code has been entered

    private PrisonCell prisonCellScript;
    private TMP_Text cageText;

    // Cooldown timer to debounce keypad-opening input
    private float openCooldown = 0f; // seconds

    void Start()
    {
        enemyMovementScript = Object.FindFirstObjectByType<EnemyMovement>();

        // Get reference to the PrisonCell script.
        // (This works even if the PrisonCell GameObject is inactive because we used Resources.FindObjectsOfTypeAll in that script.)
        prisonCellScript = Object.FindFirstObjectByType<PrisonCell>();

        // Find and store the Cage Text object.
        cageText = GameObject.Find("CageText").GetComponent<TMP_Text>();

        // Make sure the text is hidden at the start.
        cageText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Decrease the cooldown timer, if active.
        if (openCooldown > 0f)
        {
            openCooldown -= Time.deltaTime;
        }

        // If the code is correct, force the cage text to be hidden and exit.
        if (codeCorrect)
        {
            if (cageText.gameObject.activeSelf)
            {
                cageText.gameObject.SetActive(false);
            }
            return; // Skip further processing so the text won't be re-enabled.
        }

        if (enemyMovementScript.playerIsDetected == true)
        {
            cageText.gameObject.SetActive(false);
            return;
        }

        // Otherwise, if the player is in the area and the keypad UI is not active,
        // and we're not in a cooldown period, then show the text.
        if (inCageKeypadArea && !prisonCellScript.keypadUI.activeSelf && openCooldown <= 0f)
        {
            cageText.gameObject.SetActive(true);

            // Open the keypad when the player releases the Submit button.
            if (Input.GetKeyUp(KeyCode.X) || Input.GetButtonUp("Submit"))
            {
                OpenKeypad();
                // Set a cooldown to ignore any further input from the same physical press.
                openCooldown = 0.3f;
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inCageKeypadArea = true;

            // Show the text if the keypad hasn't been solved yet.
            if (!codeCorrect)
            {
                cageText.gameObject.SetActive(true);
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inCageKeypadArea = false;
            cageText.gameObject.SetActive(false); // Hide text when leaving.
        }
    }

    public void OpenKeypad()
    {
        if (!codeCorrect) // Only allow opening if the correct code hasn’t been entered.
        {
            cageText.gameObject.SetActive(false); // Hide the text.
            prisonCellScript.ShowKeypad(); // Tell the PrisonCell to open the keypad UI.
        }
    }

    // Call this from the keypad script when the player enters the correct code.
    public void CorrectCodeEntered()
    {
        codeCorrect = true;
        cageText.gameObject.SetActive(false); // Hide text permanently.
    }


    // Allow external scripts (like PrisonCell.Cancel()) to set a cooldown.
    public void SetOpenCooldown(float cooldown)
    {
        openCooldown = cooldown;
    }
}
