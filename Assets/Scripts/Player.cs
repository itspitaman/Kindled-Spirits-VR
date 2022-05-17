using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;


public class Player : MonoBehaviour
{
    XROrigin xrOrigin;
    public CapsuleCollider playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        xrOrigin = GetComponent<XROrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPos = xrOrigin.CameraInOriginSpacePos;
        playerCollider.height = Mathf.Clamp(cameraPos.y, 1f, 2.5f);
        playerCollider.center = new Vector3(cameraPos.x, playerCollider.height / 2, cameraPos.z);
        
    }
}
