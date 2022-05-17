// using.XRT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRTerra_CameraController : MonoBehaviour
{
    bool isDragging;
    public float movementX = 4f;
    public float movementY = 4f;
    // Start is called before the first frame update
    void Start()
    {       
    }

    // Update is called once per frame
    void Update()
    {       
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isDragging = true;
        }
        else if (context.canceled)
        {
            isDragging = false;
        }
    }

    public void MouseX(InputAction.CallbackContext context)
    {
        if (isDragging)
        {
            float mouseXDelta = context.ReadValue<float>() * Time.deltaTime * movementX;
            Vector3 desiredRotation = transform.eulerAngles + new Vector3(0f, mouseXDelta, 0f);
            //transform.Rotate(0f, mouseXDelta, 0f);
            transform.rotation = Quaternion.Euler(desiredRotation);
        }
    }

    public void MouseY(InputAction.CallbackContext context)
    {
        if (isDragging)
        {
            float mouseYDelta = context.ReadValue<float>() * Time.deltaTime * movementY;
            Vector3 desiredRotation = transform.eulerAngles + new Vector3(-mouseYDelta, 0f, 0f);

            if(desiredRotation.x > 180)
            {
                desiredRotation.x -= 360;
            }
            desiredRotation.x = Mathf.Clamp(desiredRotation.x, -85f, 85f);
            transform.rotation = Quaternion.Euler(desiredRotation);
        }
    }
}
