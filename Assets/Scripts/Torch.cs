using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Normal.Realtime;

public class Torch : MonoBehaviour
{
    public SphereCollider warmthCollider;
    public SphereCollider toastingCollider;
    public GameObject torchFlame;
    public bool isLit;
    float originalRadius;
    AudioSource FireSound;

    //Multiplayer Stuff
    public RealtimeView _realtimeView;
    public RealtimeTransform _realtimeTransform;

    public 


    // Start is called before the first frame update
    void Start()
    {
        FireSound = GetComponent<AudioSource>();
        FireSound.Play();
        torchFlame.SetActive(true);
        isLit = true;
    }

    // Update is called once per frame
    void Update()
    {
        originalRadius = warmthCollider.radius;
    }

    public void PickupTorch(SelectEnterEventArgs args)
    {
        warmthCollider.radius = 0.1f;
        _realtimeTransform.RequestOwnership();
        _realtimeView.RequestOwnership();
    }

    public void DropTorch(SelectExitEventArgs args)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
        {
            transform.position = hit.point;
            transform.rotation = Quaternion.Euler(hit.normal);
        }

        warmthCollider.radius = originalRadius;
    }

    public void TurnOnTorch()
    {
        Debug.Log("Torch should turn on!");
        isLit = true;
        torchFlame.SetActive(true);
        warmthCollider.enabled = true;
        FireSound.Play();
    }

    public void TurnOffTorch()
    {
        Debug.Log("Torch should turn off!");
        isLit = false;
        torchFlame.SetActive(false);
        warmthCollider.enabled = false;
        FireSound.Stop();
    }
}
