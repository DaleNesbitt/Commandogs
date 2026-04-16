using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PrisonCell : MonoBehaviour
{
    public GameObject keypadUI;  // The entire keypad UI panel
    public TMP_Text inputText;   // The display for entered numbers
    public Button[] keypadButtons; // Array of buttons for the keypad
    private EnemyMovement enemyMovementScript;


    private string correctCode = "7744";  // The correct code
    private string currentInput = "";     // Stores player input
    private int attempts = 0;             // Number of failed attempts
    private int maxAttempts = 3;          // Max tries before alarm
    private bool codeCorrect = false;     // Track if the code was entered correctly

    private void Start()
    {
        enemyMovementScript = Object.FindFirstObjectByType<EnemyMovement>();
        keypadUI.SetActive(false);  // Ensure the keypad is hidden at game start

        // Optionally, assign buttons manually in the inspector or find them dynamically
        if (keypadButtons.Length == 0)
        {
            keypadButtons = keypadUI.GetComponentsInChildren<Button>();
        }

        // Ensure the first button is selected when the keypad UI is shown
        EventSystem.current.SetSelectedGameObject(keypadButtons[0].gameObject);
    }

    // Called when a number button is pressed (0-9)
    public void EnterDigit(string digit)
    {
        AudioManager.Instance.PlaySound("Beep");  // Play beep on digit press
        if (currentInput.Length < 4)  // Limit to 4 digits
        {
            currentInput += digit;
            UpdateDisplay();
        }
    }

    // Called when ENTER is pressed
    public void Enter()
    {
        AudioManager.Instance.PlaySound("Beep");  // Play beep on enter press
        if (codeCorrect || attempts >= maxAttempts) return; // Prevent interaction if already unlocked or locked

        if (currentInput == correctCode)
        {
            OpenDoor();
            Clear();
        }
        else
        {
            attempts++;
            if (attempts >= maxAttempts)
            {
                TriggerAlarm();
            }
            else
            {
                currentInput = "Incorrect";
                UpdateDisplay(); // Ensure the text updates before clearing
                Invoke("Clear", 1.5f); // Delay clearing after 1.5 seconds

            }
        }
    }

    // Clears the current input (CLEAR button)
    public void Clear()
    {
        AudioManager.Instance.PlaySound("Beep");  // Play beep on clear press
        currentInput = "";
        UpdateDisplay();
    }

    // Closes the keypad and resets input (CANCEL button)
    public void Cancel()
    {
        AudioManager.Instance.PlaySound("Beep");  // Play beep on cancel press
        Debug.Log("Cancel button pressed.");

        if (codeCorrect) return; // If the code is correct, don't allow reopening

        // Clear the input and hide the keypad.
        Clear();
        keypadUI.SetActive(false);
        Debug.Log("Keypad UI closed.");

        // Find the CageKeyPadScript and set a short cooldown so that the release event isn't processed.
        CageKeyPadScript cageKeyPad = Object.FindFirstObjectByType<CageKeyPadScript>();

        if (cageKeyPad != null)
        {
            cageKeyPad.SetOpenCooldown(0.3f);
        }
    }

    // Updates the input display
    private void UpdateDisplay()
    {
        inputText.text = currentInput;
    }

    // This method is triggered when the correct code is entered
    private void OpenDoor()
    {
        Debug.Log("Door opened!");
        codeCorrect = true;
        keypadUI.SetActive(false);

        // Synchronize the state with CageKeyPadScript so the cage text is hidden
        CageKeyPadScript cageKeyPad = Object.FindFirstObjectByType<CageKeyPadScript>();
        if (cageKeyPad != null)
        {
            cageKeyPad.CorrectCodeEntered();
        }
    }


    // This method is triggered after 3 failed attempts
    private void TriggerAlarm()
    {
        keypadUI.SetActive(false);
        Clear();
        enemyMovementScript.PlayerDetected();
    }

    // Optionally, you can call this method to focus on the keypad when shown
    public void ShowKeypad()
    {
        // Only allow the keypad to be opened manually if the correct code hasn't been entered
        if (!codeCorrect)
        {
            keypadUI.SetActive(true); // Show the keypad when triggered
            EventSystem.current.SetSelectedGameObject(keypadButtons[0].gameObject); // Focus on the first button
            Debug.Log("Keypad opened.");
        }
        else
        {
            Debug.Log("Code is correct, keypad remains closed.");
        }
    }
}
