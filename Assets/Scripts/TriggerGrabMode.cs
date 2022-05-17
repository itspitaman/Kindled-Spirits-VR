using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGrabMode : MonoBehaviour
{
    public Backpack.GrabMode whichGrabMode;
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Controller"))
        {
            if(whichGrabMode == Backpack.GrabMode.Kindling)
            {
                Backpack.Instance.SetGrabModeKindling();
                audioSource.Play();
            }
            else if(whichGrabMode == Backpack.GrabMode.Marshmallow)
            {
                Backpack.Instance.SetGrabModeMarshmallow();
                audioSource.Play();
            }
            else if(whichGrabMode == Backpack.GrabMode.Key)
            {
                Backpack.Instance.SetGrabModeKey();
                audioSource.Play();
            }
        }
    }
}
