using System.Collections;
using UnityEngine;
using TMPro;

public class CameraSwitcher : MonoBehaviour
{
    bool isSwitching = false;  // Prevent multiple switches happening simultaneously

    // ARRAY OF CAMERAS
    public Camera[] cameras;

    // ARRAY OF CAMERA SWITCH KEYS
    public KeyCode[] cameraKeys = new KeyCode[] {KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3};

    // Variable to keep track of the currently active camera
    public Camera currentCamera;

    public TextMeshProUGUI cameraLabel;

    public string[] cameraNames;

    void Start()
    {
        // CONNECT ALL DISPLAYS
        Debug.Log("displays connected: " + Display.displays.Length);

        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }

        // Initialize currentCamera to the first camera
        currentCamera = cameras[0]; // or any default camera

        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].enabled = false;
        }

        UpdateCameraLabel(0);
    }

    void Update()
    {
        // CODE FOR SWITCHING CAMERAS
        for (int i = 0; i < cameraKeys.Length; i++)
        {
            if (Input.GetKeyDown(cameraKeys[i]) && !isSwitching)
            {
                AudioManager.Instance.PlaySound("Static");
                // Disable the currently active camera before switching to glitched camera
                currentCamera.enabled = false;

                // Start the coroutine for switching to glitched camera first and then to the desired camera
                StartCoroutine(SwitchToCamera3AndThenDesired(i));
            }
        }
    }

    private IEnumerator SwitchToCamera3AndThenDesired(int cameraIndex)
    {
        isSwitching = true;

        // Switch to glitched camera
        if (cameraIndex + 18 < cameras.Length)
        {
            Debug.Log($"Switching to glitched camera at index {cameraIndex + 18}");
            cameras[cameraIndex + 18].enabled = true;
            currentCamera = cameras[cameraIndex + 18];
        }
        else
        {
            Debug.LogError($"Invalid glitched camera index: {cameraIndex + 18}");
            yield break;
        }

        yield return new WaitForSeconds(0.5f);

        // Disable glitched camera
        cameras[cameraIndex + 18].enabled = false;

        // Switch to the desired camera
        if (cameraIndex < cameras.Length)
        {
            Debug.Log($"Switching to desired camera at index {cameraIndex}");
            cameras[cameraIndex].enabled = true;
            currentCamera = cameras[cameraIndex];
            UpdateCameraLabel(cameraIndex);
        }
        else
        {
            Debug.LogError($"Invalid desired camera index: {cameraIndex}");
        }

        isSwitching = false;
    }


    void UpdateCameraLabel(int index)
    {
        if (index >= 0 && index < cameraNames.Length)
        {
            cameraLabel.text = $"Cam {(index + 1).ToString("00")} - {cameraNames[index]}";
        }
        else
        {
            cameraLabel.text = $"Cam {(index + 1).ToString("00")}";
        }
    }

}
