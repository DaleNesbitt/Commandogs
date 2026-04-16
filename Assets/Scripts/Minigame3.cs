using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Minigame3 : MonoBehaviour
{
    public GameObject[] lights;
    public GameObject[] indicatorLights;
    Color lightOff;
    Color lightOn;

    public bool gameStart;
    public bool gameWon;
    bool gameStart2;
    float speed;
    int counter;

    int currentLight;

    void Start()
    {
        lightOff = new Color(0.4f, 0.4f, 0.4f);
        lightOn = new Color(1, 1, 1);
        ResetGame();

        gameStart = false;
        speed = 0.5f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (indicatorLights[currentLight].GetComponent<Image>().color == Color.red)
            {
                NextStage();
            }
            else if (indicatorLights[currentLight].GetComponent<Image>().color == Color.green && indicatorLights[currentLight] != indicatorLights[0])
            {
                StopGame();
                ResetGame();
            }
        }

        if (counter == 9)
        {
            gameWon = true;
        }
    }

    public void StopGame()
    {
        StopAllCoroutines();
    }

    public void StartGame()
    {
        StartCoroutine(LightCycle(true, speed));
        gameStart = true;
    }

    public void NextStage()
    {
        gameStart2 = true;
        indicatorLights[currentLight].GetComponent<Image>().color = Color.green;
        lights[currentLight].GetComponent<Image>().color = lightOff;

        if (gameStart2)
        {
            counter = Mathf.Min(counter + 1, 9);
            StopGame();
            speed = Mathf.Max(0.1f, speed - 0.05f);
            StartGame();
            gameStart2 = false;
        }
    }

    public void ResetGame()
    {
        gameStart = true;
        counter = 0;
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].GetComponent<Image>().color = lightOff;
        }
        for (int i = 0; i < indicatorLights.Length; i++)
        {
            indicatorLights[i].GetComponent<Image>().color = Color.red;
        }
        speed = 0.5f;
    }

    private IEnumerator LightCycle(bool active, float speed)
    {
        while (active)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                currentLight = i;
                lights[i].GetComponent<Image>().color = lightOn;
                yield return new WaitForSeconds(speed);
                lights[i].GetComponent<Image>().color = lightOff;
            }
        }
    }
}
