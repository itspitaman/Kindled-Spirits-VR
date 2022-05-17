using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public List<Health> visibleHealths;


    // Start is called before the first frame update
    void Start()
    {
        visibleHealths.Clear(); 
    }


    private void OnTriggerEnter(Collider other)
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
            visibleHealths.Add(collidedHealth);
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
            visibleHealths.Remove(collidedHealth);
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
