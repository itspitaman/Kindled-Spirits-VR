using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    AudioSource Winning;


    // Start is called before the first frame update
    void Start()
    {
        Winning = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            SinglePlayerManager.Instance.OpenDoor();
            Winning.Play();
        }
    }
}
