using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    string currentSceneName;

    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void NewGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Options()
    {
        SceneManager.LoadScene("OptionsScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Load the main menu scene
            SceneManager.LoadScene("MenuScene");
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            Application.Quit();

        }
    }

}
