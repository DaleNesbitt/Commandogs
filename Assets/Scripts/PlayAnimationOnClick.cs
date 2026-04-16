using UnityEngine;
using UnityEngine.UI;  // Required for UI components

public class PlayAnimationOnClick : MonoBehaviour
{
    public Animator animator;   // Reference to the Animator

    private Button button;   // Button reference

    private bool toggle = false; // Track the current state

    void Start()
    {
        // Find the button in the scene by its name or tag (if you know it).
        button = GameObject.Find("iPadButton")?.GetComponent<Button>();  // Replace "ButtonName" with your actual button's name or tag.

        // If you have multiple buttons, you can find the specific one using tags:
        // button = GameObject.FindGameObjectWithTag("YourTag")?.GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(ToggleAnimation);
        }
        else
        {
            Debug.LogError("Button not found! Please make sure your button is in the scene.");
        }
    }

    // Function to toggle and play the animation
    public void ToggleAnimation()
    {
        if (button == null) return; // Make sure the button is assigned

        // Log to check toggle state during button press
        Debug.Log("Toggle clicked, current state: " + toggle);

        if (toggle == false)
        {
            animator.SetBool("Toggle", true);  // Trigger iPadIn animation
            toggle = true;  // Set state to "iPadIn"
        }
        else
        {
            animator.SetBool("Toggle", false);  // Trigger iPadOut animation
            toggle = false;  // Set state to "iPadOut"
        }
    }
}
