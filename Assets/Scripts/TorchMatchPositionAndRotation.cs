using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class TorchMatchPositionAndRotation : MonoBehaviour
{
    public Transform torchPhysics;
    RealtimeTransform _realtimeTransform;

    // Start is called before the first frame update
    void Start()
    {
        _realtimeTransform = GetComponent<RealtimeTransform>();   
    }

    // Update is called once per frame
    void Update()
    {
        if(_realtimeTransform.isOwnedLocallyInHierarchy)
        {
            transform.position = torchPhysics.position;
            transform.rotation = torchPhysics.rotation;
        }
        else
        {
            torchPhysics.position = transform.position;
            torchPhysics.rotation = transform.rotation;
        }
    }
}
