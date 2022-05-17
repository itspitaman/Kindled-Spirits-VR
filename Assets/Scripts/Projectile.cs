using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public GameObject destructionParticlesPrefab;
    AudioSource ProjectileSounds;


    // Start is called before the first frame update
    void Start()
    {
        ProjectileSounds = GetComponent<AudioSource>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        Health collidedHealth = collision.gameObject.GetComponent<Health>();
        if(collidedHealth == null)
        {
            collidedHealth = collision.gameObject.GetComponentInParent<Health>();
        }
        if(collidedHealth == null)
        {
            collidedHealth = collision.gameObject.GetComponentInChildren<Health>();
        }

        if(collidedHealth == null)
        {
          //  Debug.Log("Collided with something, but there wasn't a health component.");
        }
        else
        {
            collidedHealth.TakeDamage(damage);
            //Debug.Log(gameObject.name + " dealt " + damage + " damaged to " + collidedHealth.gameObject.name);
            Instantiate(destructionParticlesPrefab, transform.position, transform.rotation);
            ProjectileSounds.Play();
            Destroy(gameObject, 2f);
        }
    }

  

    // Update is called once per frame
    void Update()
    {
        
    }
}
