using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewScene : MonoBehaviour
{
    public AudioSource menuAudioSource;

    public void LoadTheScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        menuAudioSource.Play();
    }
}
