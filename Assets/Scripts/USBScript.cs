using System.Collections;
using TMPro;
using UnityEngine;

public class USBScript : MonoBehaviour
{
    private GameObject pc;
    private PCScript pcScript;
    private GameObject redLight;
    private GameObject floppy;
    private TMP_Text redFloppyText;
    private bool floppyCollected = false;

    void Start()
    {
        pc = GameObject.Find("PC");
        pcScript = pc.GetComponent<PCScript>();
        redLight = GameObject.Find("RedLight");

        floppy = GameObject.Find("Floppy");
        redFloppyText = GameObject.Find("RedFloppyText").GetComponent<TMP_Text>();
        redFloppyText.gameObject.SetActive(false); // Ensure the text is initially hidden
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !floppyCollected) // Ensure this only triggers once
        {
            floppyCollected = true;
            StartCoroutine(FloppyCollectedEvent());
            redLight.SetActive(false);
            floppy.SetActive(false);
            pcScript.USBCollected = true;
        }
    }

    private IEnumerator FloppyCollectedEvent()
    {
        redFloppyText.gameObject.SetActive(true);
        AudioManager.Instance.PlaySound("Floppy");
        yield return new WaitForSeconds(3);
        redFloppyText.gameObject.SetActive(false);
    }
}
