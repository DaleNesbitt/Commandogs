using System.Collections;
using TMPro;
using UnityEngine;

public class keycardScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject pc;
    private PCScript pcScript;
    private GameObject keycardLight;
    private GameObject keycard;
    [SerializeField] private TMP_Text secuirtyCard;

    void Start()
    {
        pcScript = pc.gameObject.GetComponent<PCScript>();
        keycardLight = GameObject.Find("KeyCardLight");
        secuirtyCard.gameObject.SetActive(false);
        keycard = GameObject.Find("Card");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        StartCoroutine(keyCardInteraction());
    }

    private IEnumerator keyCardInteraction()
    {
        AudioManager.Instance.PlaySound("CardPickup");
        secuirtyCard.gameObject.SetActive(true);
        keycardLight.gameObject.SetActive(false);
        pcScript.KeycardCollected = true;
        keycard.gameObject.SetActive(false);
        yield return new WaitForSeconds(3);
        secuirtyCard.gameObject.SetActive(false);
    }
}
