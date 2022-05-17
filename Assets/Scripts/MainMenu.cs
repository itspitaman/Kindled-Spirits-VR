using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject singlePlayerPanel;
    public GameObject multiPlayerPanel;
    AudioSource menuAudioSource;

    void Start()
    {
        menuAudioSource = GetComponent<AudioSource>();
    }

    public void GoToSinglePlayer()
    {
        mainMenuPanel.SetActive(false);
        singlePlayerPanel.SetActive(true);
        multiPlayerPanel.SetActive(false);
        menuAudioSource.Play();
    }

    public void GoToMainMenu()
    {
        mainMenuPanel.SetActive(true);
        singlePlayerPanel.SetActive(false);
        multiPlayerPanel.SetActive(false);
        menuAudioSource.Play();
    }

    public void GoToMultiPlayer()
    {
        mainMenuPanel.SetActive(false);
        singlePlayerPanel.SetActive(false);
        multiPlayerPanel.SetActive(true);
        menuAudioSource.Play();
    }

    public void QuitGame()
    {
        Debug.Log("Game shut quit now");
        Application.Quit();
    }

    public void PlaySound()
    {
        menuAudioSource.Play();
    }
}
