using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireManager : MonoBehaviour
{
    public static CampfireManager Instance;

    public List<Health> campfireHealths;
    public int winHealthThreshold;

    public GameObject winCanvas;
    bool wonGame;

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
        winCanvas.SetActive(false);
        wonGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool allCampfiresHealthy = true;

        foreach (Health campfire in campfireHealths)
        {
            if (campfire.currentHitPoints < winHealthThreshold)
            {
                allCampfiresHealthy = false;
            }
        }
        
        if (allCampfiresHealthy && !wonGame)
        {
            wonGame = true;
            EnemyManager.Instance.KillAllEnemies();
            winCanvas.SetActive(true);
        }
    }
}
