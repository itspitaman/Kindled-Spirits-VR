using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    public int coldDamage;
    public float timeBetweenTicks;
    public GameObject player;

    float timer;
    Health playerHealth;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    void Start()
    {
        playerHealth = player.GetComponentInChildren<Health>();
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!playerHealth.isDead && (timer >= timeBetweenTicks))
        {
            DealColdDamage();
            timer = 0f;
        }
    }

    void DealColdDamage()
    {
        playerHealth.TakeDamage(coldDamage);
    }
}
