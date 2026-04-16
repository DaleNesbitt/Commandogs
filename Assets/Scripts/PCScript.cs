using System.Collections;
using TMPro;
using UnityEngine;

public class PCScript : MonoBehaviour
{
    public bool USBCollected;
    public bool PCInteracted;
    public bool KeycardCollected;
    // So I can populate the fields in the inspector without "finding" all the objects in code.
    [SerializeField] private GameObject forceField;
    [SerializeField] private GameObject exitDoor;
    [SerializeField] private TMP_Text virusUploaded;
    [SerializeField] private TMP_Text noFloppyText;
    [SerializeField] private TMP_Text uploadVirus;

    private bool isPlayerInTrigger;

    void Start()
    {
        // Set interactables to false
        USBCollected = false;
        KeycardCollected = false;
        PCInteracted = false;

        forceField.gameObject.SetActive(true);
        virusUploaded.gameObject.SetActive(false);
        noFloppyText.gameObject.SetActive(false);
        uploadVirus.gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !PCInteracted)
        {
            isPlayerInTrigger = true;
            noFloppyText.gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            noFloppyText.gameObject.SetActive(false);
            uploadVirus.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInTrigger && USBCollected && !PCInteracted)
        {
            noFloppyText.gameObject.SetActive(false);
            uploadVirus.gameObject.SetActive(true);

            if ((Input.GetKeyUp(KeyCode.X) || Input.GetButtonDown("Submit")))
            {
                StartCoroutine(PCInteraction());
            }
        }
    }

    private IEnumerator PCInteraction()
    {
        PCInteracted = true;
        forceField.gameObject.SetActive(false);
        uploadVirus.gameObject.SetActive(false);
        virusUploaded.gameObject.SetActive(true);
        AudioManager.Instance.PlaySound("VirusUploaded");
        yield return new WaitForSeconds(3);
        virusUploaded.gameObject.SetActive(false);
        AudioManager.Instance.PlaySound("ForcefieldDisabled");

    }
}
