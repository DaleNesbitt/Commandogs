using TMPro;
using UnityEngine;

public class EndGameScript : MonoBehaviour
{
    private bool goodbyePlayed = false;
    private LevelManager levelManager;
    private AltPlayerMovement altPlayerMovement;
    private TMP_Text missionComplete;
    private Animator missionCompleteAnimator;

    void Start()
    {
        missionComplete = GameObject.Find("MissionComplete").GetComponent<TMP_Text>();
        missionComplete.gameObject.SetActive(false); // Ensure the text is initially hidden
        missionCompleteAnimator = missionComplete.GetComponent<Animator>();
        levelManager = FindAnyObjectByType<LevelManager>();
        altPlayerMovement = FindAnyObjectByType<AltPlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        if (!goodbyePlayed)
        {
            int randomGoodbyeMessage = Random.Range(1, 4);
            string goodbyeClip = "Goodbye" + randomGoodbyeMessage.ToString();
            altPlayerMovement.runStopped = true;
            AudioManager.Instance.PlaySound(goodbyeClip);
            goodbyePlayed = true;
            missionComplete.gameObject.SetActive(true);
            missionCompleteAnimator.SetBool("MissionComplete", true);


            // Get the length of the audio clip
            float goodbyeLength = AudioManager.Instance.GetSoundLength(goodbyeClip);

            // Delay the next action after the audio finishes
            Invoke(nameof(GameComplete), goodbyeLength);
        }
    }

    private void GameComplete()
    {
        levelManager.MainMenu();
    }
}