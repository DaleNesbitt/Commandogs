using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Minigame1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Button[] buttons; // array of buttons

    public Button[] shuffledButtons; // shuffled array of buttons

    //variables needed
    int counter = 0;
    int timeLimit = 0;
    public bool gameStarted;
    bool timerActive;
    float currentTime;
    bool timerUp;
    public bool gameWon;

    public TMP_Text timerText;
    void Start()
    {
        gameWon = false;
        gameStarted = true; //remove this when doing the actual game, game will start once gamestarted = true
    }

    // Update is called once per frame
    void Update()
    {
        //if game is not started start game
        if (!gameStarted && !gameWon)
        {
            GameStart();
        }

        //timer code
        if (timerActive)
        {
            currentTime = currentTime - Time.deltaTime;
        }
        if (!timerUp)
        { 
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        timerText.text = time.ToString(@"ss\:fff");
        }
    }


    public void GameStart()
    {
        StopAllCoroutines(); // Stop all existing coroutines
        gameStarted = true;
        timerUp = false;
        gameWon = false;
        counter = 0;
        timerActive = false; // Ensure the timer is reset
        currentTime = 7;

        shuffledButtons = buttons.OrderBy(a => UnityEngine.Random.Range(0, 100)).ToArray();

        for (int i = 1; i < 11; i++)
        {
            shuffledButtons[i - 1].GetComponentInChildren<TMP_Text>().text = i.ToString();
            shuffledButtons[i - 1].interactable = true;
            shuffledButtons[i - 1].image.color = Color.white;
        }

        StartCoroutine(StartTimer(14)); // Start fresh timer
    }




    public void pressButton(Button button) //this is called on the button onClick()
    {
        if (int.Parse(button.GetComponentInChildren<TMP_Text>().text) - 1 == counter) //check if button pressed is the correct button
        {
            counter++; //increase counter to check for next button
            button.interactable = false; //disable that button
            button.image.color = Color.green; // mark the button as correct

            if (counter == 10) //if game won
            {
                StartCoroutine(presentResult(true));
            }
        }
        else // if game lost
        {
            StartCoroutine(presentResult(false));
        }
    }

    private IEnumerator TimeOver(bool timeUp)
    {
        if (timeUp)
        {
            foreach (var button in shuffledButtons)
            {
                button.image.color = Color.red;
                button.interactable = false;
            }
            timerUp = true;
            timerText.text = "00:000";
        }

        yield return new WaitForSeconds(2f);

        StopAllCoroutines(); // Stop any lingering coroutines before restarting
        GameStart();
    }


    private IEnumerator presentResult(bool success)
    {
        timerUp = true; // Stop timer updates
        timerActive = false; // Ensure timer is inactive

        if (!success) // If the player fails
        {
            foreach (var button in shuffledButtons) // Mark buttons as red & disable them
            {
                button.image.color = Color.red;
                button.interactable = false;
            }
            yield return new WaitForSeconds(2f);
            GameStart(); // Restart game
        }
        else // If the player wins
        {
            gameWon = true; // Mark game as won
            timerActive = false; // Stop the timer from running
            this.gameObject.SetActive(false); // Hide the minigame
            StopAllCoroutines(); // Stop everything
        }
    }


    private IEnumerator StartTimer(int time)
    {
        if (timerActive) yield break; // Prevent multiple timers from running
        timeLimit = time;
        timerActive = true;
        currentTime = timeLimit;

        while (currentTime > 0)
        {
            if (gameWon) yield break; // Stop timer if the game is won
            yield return null;
            currentTime -= Time.deltaTime;
            timerText.text = TimeSpan.FromSeconds(Mathf.Max(currentTime, 0)).ToString(@"ss\:fff"); // Prevent negative display
        }

        timerActive = false; // Ensure timer is marked inactive
        if (!gameWon) StartCoroutine(TimeOver(true));
    }
}
