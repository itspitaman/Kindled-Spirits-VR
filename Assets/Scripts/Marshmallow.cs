using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marshmallow : MonoBehaviour
{
    public enum ToastingState { Untoasted, LightlyToasted, WellDone, Burnt}
    public ToastingState currentState;

    public bool isToasting;
    public float toastTimer;
    public float timeTillLightlyToasted = 1f;
    public float timeTillWellDone = 2f;
    public float timeTillBurnt = 3f;
    public int untoastedDamage, lightToastDamage, wellDoneDamage, burntDamage;

    public Material untoastedMat, lightToastMat, wellDoneMat, burntMat;
    public MeshRenderer[] renderers;

    public GameObject fireFX;

    Projectile marshProjectile;

    // Start is called before the first frame update
    void Start()
    {
        toastTimer = 0f;
        marshProjectile = GetComponent<Projectile>();
        currentState = ToastingState.Untoasted;
        marshProjectile.damage = untoastedDamage;
        SetMaterial(untoastedMat);
    }

    // Update is called once per frame
    void Update()
    {
        if (isToasting)
        {
            toastTimer += Time.deltaTime;
            fireFX.SetActive(true);
        }
        else 
        {
            fireFX.SetActive(false);
        }

        if (toastTimer > timeTillBurnt)
        {
            currentState = ToastingState.Burnt;
            marshProjectile.damage = burntDamage;
            SetMaterial(burntMat);
        }
        else if (toastTimer > timeTillWellDone)
        {
            currentState = ToastingState.WellDone;
            marshProjectile.damage = wellDoneDamage;
            SetMaterial(wellDoneMat);
        }
        else if (toastTimer > timeTillLightlyToasted)
        {
            currentState = ToastingState.LightlyToasted;
            marshProjectile.damage = lightToastDamage;
            SetMaterial(lightToastMat);
        }
        
    }


    void SetMaterial(Material whichMaterial)
    {
        foreach(MeshRenderer rend in renderers)
        {
            rend.material = whichMaterial;
        }
    }



}
