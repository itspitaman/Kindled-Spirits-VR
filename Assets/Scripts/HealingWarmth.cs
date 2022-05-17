using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingWarmth : MonoBehaviour
{
    public List<Health> healthsToHeal;

    public float timeBetweenTicks;
    public int healTickAmount;
    float timer;

    private void Start()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeBetweenTicks)
        {
            timer = 0;

            if (healthsToHeal.Count > 0)
            {
                Debug.Log(healthsToHeal.Count);
                foreach (Health health in healthsToHeal)
                {
                    Debug.Log(health);
                    if(!health.isDead)
                    {
                    health.HealDamage(healTickAmount);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Something entered the campfire.");

        Health collidedHealth = other.gameObject.GetComponent<Health>();

        if (collidedHealth == null)
        {
            collidedHealth = other.gameObject.GetComponentInParent<Health>();
        }

        if (collidedHealth == null)
        {
            collidedHealth = other.gameObject.GetComponentInChildren<Health>();
        }

        if (collidedHealth == null)
        {
            //Debug.Log("Collided with something, but there wasn't a health component.");
        }
        else
        {
            if (!collidedHealth.isEnemyGolem && !collidedHealth.CompareTag("Campfire"))
            {
                healthsToHeal.Add(collidedHealth);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Health collidedHealth = other.gameObject.GetComponent<Health>();

        if (collidedHealth == null)
        {
            collidedHealth = other.gameObject.GetComponentInParent<Health>();
        }
        if (collidedHealth == null)
        {
            collidedHealth = other.gameObject.GetComponentInChildren<Health>();
        }

        if (collidedHealth == null)
        {
            //Debug.Log("Collided with something, but there wasn't a health component.");
        }
        else
        {
            if (!collidedHealth.isEnemyGolem)
            {
                healthsToHeal.Remove(collidedHealth);
            }
        }
    }
}
