using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using Normal.Realtime;

public class Health : MonoBehaviour
{
    public int currentHitPoints;
    public int maxHitPoints = 100;
    public bool isEnemyIcicle;
    public bool isEnemyGolem;
    public bool isCampfire;
    public bool canRespawn; // This is checked only if they are a player
    public bool isDead;

    public Slider healthSlider;

    public ActionBasedContinuousMoveProvider moveProvider;
    float originalMoveSpeed;
    public GameObject respawnCanvas;
    public Text respawnTimerText;
    public float respawnTime;
    float timeSpentRespawning;
    public Transform playerTransform;
    public Transform respawnTransform;
    public InputActionReference[] respawnAction;

    // SnowFolk class reference for triggering animations
    SnowFolk enemyGolem;
    IceCicleEnemy enemyCicle;
 
    // Multiplayer Stuff
    EnemyHealthSync _enemyHealthSync;
    CampfireHealthSync _campfireHealthSync;

    public GameObject playerHealthbarPrefab;
    public Transform playerHealthbarLocation;
    Transform playerHealthBar;
    Realtime _realtime;
    bool wasAlreadyConnected;

    // Start is called before the first frame update
    public AudioSource source;
    public AudioClip clip1;
    public AudioClip clip2;


    private void Awake()
    {
        if (isCampfire)
        {
            _campfireHealthSync = GetComponent<CampfireHealthSync>();
        }

        if (isEnemyGolem)
        {
            //EnemyManager.Instance.enemyHealths.Add(this);
             enemyGolem = GetComponent<SnowFolk>();
            _enemyHealthSync = GetComponent<EnemyHealthSync>();

          

        }
        else if (isEnemyIcicle)
        {
            //EnemyManager.Instance.enemyHealths.Add(this);
            enemyCicle = GetComponent<IceCicleEnemy>();
            _enemyHealthSync = GetComponent<EnemyHealthSync>();
        }
    }

    void Start()
    {
        healthSlider.maxValue = maxHitPoints;
        healthSlider.value = currentHitPoints;

        if(canRespawn)
        {
            originalMoveSpeed = moveProvider.moveSpeed;
            respawnCanvas.SetActive(false);
            foreach(InputActionReference reference in respawnAction)
            {
                reference.action.performed += TriggerRespawn;
            }
        }

        if (canRespawn)
        {
            _realtime = FindObjectOfType<Realtime>();
            if (_realtime.connected)
            {
                wasAlreadyConnected = true;
                SpawnInPlayerHealthBar();
            }
            else
            {
                wasAlreadyConnected = false;
                _realtime.didConnectToRoom += DidConnectToRoom;
            }
        }
    }

    void DidConnectToRoom(Realtime realtime)
    {
        SpawnInPlayerHealthBar();
    }


    void SpawnInPlayerHealthBar()
    {
        playerHealthBar = Realtime.Instantiate(playerHealthbarPrefab.name,
            playerHealthbarLocation.position,
            playerHealthbarLocation.rotation,
            Realtime.InstantiateOptions.defaults).transform;
    }


    private void OnDestroy()
    {
        if(canRespawn)
        {
            foreach (InputActionReference reference in respawnAction)
            {
                reference.action.started -= TriggerRespawn;
            }
            if(!wasAlreadyConnected)
            {
                _realtime.didConnectToRoom -= DidConnectToRoom;
            }
        }
    }

    public void TakeDamage(int takenDamage)
    {
        currentHitPoints = Mathf.Max(0, currentHitPoints - takenDamage);
        //Debug.Log(gameObject.name + " took " + takenDamage + " damage and is now at a health of: " + currentHitPoints);
        UpdateHealthSlider();

        if (isCampfire)
        {
            _campfireHealthSync.UpdateCampfireModelHealth();
        }

        if (canRespawn)
        {
            float percentHealth = (float)currentHitPoints / maxHitPoints;
            playerHealthBar.localScale = new Vector3(percentHealth, 1, 1);
        }

        if (currentHitPoints <= 0)
        {
            Die();
        }

        if (isEnemyGolem && !isDead)
        {
            _enemyHealthSync.UpdateModelHealth();
            enemyGolem.currentState = SnowFolk.SnowfolkState.Damaged;
            source.PlayOneShot(clip1);
        }
       
        else if (isEnemyIcicle && !isDead)
        {
            _enemyHealthSync.UpdateModelHealth();
            enemyCicle.currentState = IceCicleEnemy.IcicleState.Damaged;
            source.PlayOneShot(clip1);
        }
        
    }

    public void HealDamage(int healing) 
    {
        currentHitPoints = Mathf.Min(maxHitPoints, currentHitPoints + healing);
        //Debug.Log(gameObject.name + " healed " + healing + " damage and is now at a health of: " + currentHitPoints);
        UpdateHealthSlider();

        if (isCampfire)
        {
            _campfireHealthSync.UpdateCampfireModelHealth();
        }

        if (canRespawn)
        {
            float percentHealth = (float)currentHitPoints / maxHitPoints;
            playerHealthBar.localScale = new Vector3(percentHealth, 1, 1);
        }

        if (isEnemyGolem && !isDead)
        {
            _enemyHealthSync.UpdateModelHealth();
        }

        else if (isEnemyIcicle && !isDead)
        {
            _enemyHealthSync.UpdateModelHealth();
        }

      
    }

    public void UpdateHealthSlider()
    {
        healthSlider.value = currentHitPoints;
    }

    public void Die()
    {
        isDead = true;
        
        if (isEnemyGolem)
        {
            source.PlayOneShot(clip2);
            //Debug.Log("Enemy Down!");
            EnemyManager.Instance.enemyHealths.Remove(this);

            // Changed Dead() function, because methods inside snowfolk are private, only the state should be called as it is already public
            enemyGolem.currentState = SnowFolk.SnowfolkState.Dead;
            Destroy(gameObject, 4f);
        }
        else if (isEnemyIcicle)
        {
            source.PlayOneShot(clip2);
            //Debug.Log("Enemy Down!");
            EnemyManager.Instance.enemyHealths.Remove(this);

            // Changed Dead() function, because methods inside snowfolk are private, only the state should be called as it is already public
            enemyCicle.currentState = IceCicleEnemy.IcicleState.Dead;

            Destroy(gameObject, 4f);
        }

        if (canRespawn)
        {
            moveProvider.moveSpeed = 0;
            StartCoroutine(RespawnTimer());
        }
    }

    IEnumerator RespawnTimer()
    {
        respawnCanvas.SetActive(true);
        timeSpentRespawning = 0f;
        while (timeSpentRespawning < respawnTime)
        {
            timeSpentRespawning += Time.deltaTime;
            currentHitPoints = Mathf.RoundToInt(timeSpentRespawning / respawnTime * maxHitPoints);
            UpdateHealthSlider();
            respawnTimerText.text = "Respawning in: " + Mathf.RoundToInt(respawnTime - timeSpentRespawning);
            yield return null;
        }
        Respawn();
    }

    void TriggerRespawn(InputAction.CallbackContext context)
    {
        Respawn(); 
    }

    void Respawn()
    {
        if(isDead)
        {
            StopAllCoroutines();
            respawnCanvas.SetActive(false);
            moveProvider.moveSpeed = originalMoveSpeed;
            isDead = false;
            playerTransform.position = respawnTransform.position;
            playerTransform.rotation = respawnTransform.rotation;
            playerHealthBar.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canRespawn && playerHealthBar != null)
        {
            playerHealthBar.position = playerHealthbarLocation.position;
        }
    }
}
