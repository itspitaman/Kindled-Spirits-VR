using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SinglePlayerManager : MonoBehaviour
{
    public static SinglePlayerManager Instance;

    public GameObject keyWinCanvas;
    public Text countdownText;

    public float countdownTimer;
    bool wonSinglePlayerGame;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        keyWinCanvas.SetActive(false);
        wonSinglePlayerGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (wonSinglePlayerGame)
        {
            EnemyManager.Instance.KillAllEnemies();
            keyWinCanvas.SetActive(true);
            StartCoroutine(StartCountdown());
        }
    }

    public void OpenDoor()
    {
        wonSinglePlayerGame = true;
    }

    IEnumerator StartCountdown()
    {
        float timeSpentWaitting = 0f;
        while (timeSpentWaitting < countdownTimer)
        {
            Debug.Log(countdownTimer - timeSpentWaitting);
            timeSpentWaitting += Time.deltaTime;
            countdownText.text = ($"Returning to Main Menu in... ({Mathf.RoundToInt(countdownTimer - timeSpentWaitting)})");
            yield return null;
        }

        SceneManager.LoadScene("NEWMainMenu");
    }
}
