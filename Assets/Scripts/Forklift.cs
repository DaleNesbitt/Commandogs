using System.Collections;
using TMPro;
using UnityEngine;

public class Forklift : MonoBehaviour
{
    private TMP_Text keyRequiredText;
    private TMP_Text operateForksText;
    private ForkliftScript forkliftScript;
    private bool forkliftKeyArea = false;
    private Animator forkAnimator;
    private Animator forkliftBodyAnimator;
    private bool soundPlayed;

    void Start()
    {
        keyRequiredText = GameObject.Find("ForkliftKeyRequired")?.GetComponent<TMP_Text>();
        keyRequiredText?.gameObject.SetActive(false);

        operateForksText = GameObject.Find("OperateForks")?.GetComponent<TMP_Text>();
        operateForksText?.gameObject.SetActive(false);

        forkliftScript = FindAnyObjectByType<ForkliftScript>();
        forkAnimator = GameObject.Find("Forks")?.GetComponent<Animator>();
        forkliftBodyAnimator = GameObject.Find("Forklift")?.GetComponent<Animator>();
    }

    void Update()
    {
        if ((Input.GetKeyUp(KeyCode.X) || Input.GetButtonDown("Submit")) && forkliftKeyArea && !soundPlayed)
        {
            StartCoroutine(StartForklift());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (forkliftScript != null && forkliftScript.forkliftKeyCollected)
            {
                operateForksText?.gameObject.SetActive(true);
                forkliftKeyArea = true;
            }
            else
            {
                keyRequiredText?.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            keyRequiredText?.gameObject.SetActive(false);
            operateForksText?.gameObject.SetActive(false);
            forkliftKeyArea = false;
        }
    }

    IEnumerator StartForklift()
    {
        soundPlayed = true;
        AudioManager.Instance.PlaySound("ForkliftSound");
        yield return new WaitForSeconds(1.6f);

        if (forkAnimator != null)
        {

            forkAnimator.SetBool("ForksDown", true);
        }

        if (forkliftBodyAnimator != null)
        {
            forkliftBodyAnimator.SetBool("ForkliftOn", true);
        }

        yield return new WaitForSeconds(10);

        if (forkliftBodyAnimator != null)
        {
            forkliftBodyAnimator.SetBool("ForkliftOn", false);
        }
    }
}