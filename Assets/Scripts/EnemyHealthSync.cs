using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class EnemyHealthSync : RealtimeComponent<EnemyHealthModel>
{
    Health enemyHealth;
    SnowFolk enemyGolem;

    void Awake()
    {
        enemyHealth = GetComponent<Health>();
        enemyGolem = GetComponent<SnowFolk>();
    }

    //----------------------------------------------------------------------------------------------------
    // Multiplayer Stuff
    //----------------------------------------------------------------------------------------------------

    void UpdateLocalHealthFromModel()
    {
        Debug.Log("new snowfolk local health Recived");
        enemyHealth.currentHitPoints = model.health;
        enemyHealth.UpdateHealthSlider();

        if (enemyHealth != null && enemyHealth.currentHitPoints <= 0)
        {
            enemyHealth.Die();
        }
    }

    void EnemyHealthChanged(EnemyHealthModel model, int health)
    {
        Debug.Log(" snowfolk  health Changed");
        UpdateLocalHealthFromModel();
    }

    protected override void OnRealtimeModelReplaced(EnemyHealthModel previousModel, EnemyHealthModel currentModel)
    {
        Debug.Log(" onreal time model replaced");
        if (previousModel != null)
        {
            previousModel.healthDidChange -= EnemyHealthChanged;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.health = enemyHealth.currentHitPoints;
            }

            UpdateLocalHealthFromModel();
            currentModel.healthDidChange += EnemyHealthChanged;
        }
        else 
        {
            Debug.Log("theres no new model");
        }
    }

    public void UpdateModelHealth()
    {
        Debug.Log("Updating snowfolk model health");
        // This will automatically trigger the health did change event
        model.health = enemyHealth.currentHitPoints;
    }
}
