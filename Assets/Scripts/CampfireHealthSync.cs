using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class CampfireHealthSync : RealtimeComponent<CampfireHealthModel>
{
    Health campfireHealth;
    Campfire campfire;

    // Start is called before the first frame update
    void Awake()
    {
        campfireHealth = GetComponent<Health>();
        campfire = GetComponent<Campfire>();
    }

    void UpdateLocalCampfireHealth()
    {
        campfireHealth.currentHitPoints = model.health;
        campfireHealth.UpdateHealthSlider();

        if(campfireHealth.currentHitPoints <= 0)
        {
            campfire.TurnOffCampfire();
        }
        else
        {
            campfire.TurnOnCampfire();
        }
    }

    void CampfireHealthChanged(CampfireHealthModel model, int health)
    {
        UpdateLocalCampfireHealth();
    }

    protected override void OnRealtimeModelReplaced(CampfireHealthModel previousModel, CampfireHealthModel currentModel)
    {
       if (previousModel != null)
        {
            previousModel.healthDidChange -= CampfireHealthChanged;
        }

       if(currentModel != null)
       {
            if(currentModel.isFreshModel)
            {
                currentModel.health = campfireHealth.currentHitPoints;
            }

            UpdateLocalCampfireHealth();
            currentModel.healthDidChange += CampfireHealthChanged;
       }
    }

    public void UpdateCampfireModelHealth()
    {
        model.health = campfireHealth.currentHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
