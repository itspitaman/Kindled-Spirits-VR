using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleAttack : MonoBehaviour
{
    public int MeleeDamage;
    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("in hitbox");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("player detected");
            Health collidedHealth = other.gameObject.GetComponent<Health>();
            collidedHealth.TakeDamage(MeleeDamage);
        }
    }
}
