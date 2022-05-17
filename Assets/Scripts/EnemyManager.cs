using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public GameObject enemyGolemPrefab;
    public GameObject enemyIciclePrefab;
    public float chanceToSpawnIcicle;

    public List<Health> enemyHealths;
    public List<Transform> spawnTransforms;
    public List<Transform> playerTransforms;
    public float maxDistanceFromPlayer;

    public float timeBetweenSpawns = 1;
    float timer;
    public Realtime realtime;
    public int maxNumEnemies;

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
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > timeBetweenSpawns)
        {
            timer = 0f;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (enemyHealths.Count < maxNumEnemies && spawnTransforms.Count > 0 && realtime.connected)
        {
            Transform chosenTransform = spawnTransforms[Random.Range(0, spawnTransforms.Count)];

            float r = Random.Range(0f, 100f);
            //Debug.Log(r);

            if (r  <= chanceToSpawnIcicle)
            {
                // Single-Player implementation uses Instantiate
                //Instantiate(enemyIciclePrefab, chosenTransform.position, chosenTransform.rotation);

                // Multi-Player implementation uses RealTime.Instantiate
                GameObject newIcecile = Realtime.Instantiate(enemyIciclePrefab.name, chosenTransform.position, chosenTransform.rotation, Realtime.InstantiateOptions.defaults);
                enemyHealths.Add(newIcecile.GetComponent<Health>());
            }
            else
            {
                // Single-Player implementation uses Instantiate
                //Instantiate(enemyGolemPrefab, chosenTransform.position, chosenTransform.rotation);

                // Multi-Player implementation uses RealTime.Instantiate
                GameObject newGolem = Realtime.Instantiate(enemyGolemPrefab.name, chosenTransform.position, chosenTransform.rotation, Realtime.InstantiateOptions.defaults);
                enemyHealths.Add(newGolem.GetComponent<Health>());
            }
        }
    }

    //Originally called GameIsOver(), but that didnt make sense to me lol
    public void KillAllEnemies()
    {
        maxNumEnemies = 0;

        foreach (Health enemy in enemyHealths)
        {
            enemy.gameObject.SetActive(false);
            //enemy.Die();
        }

        //enemyHealths.Clear();
    }
}
