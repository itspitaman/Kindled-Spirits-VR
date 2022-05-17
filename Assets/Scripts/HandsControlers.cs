using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandsControlers : MonoBehaviour
{
    [SerializeField] InputActionReference GripInputAction;
    [SerializeField] InputActionReference TriggerInputAction;

    Animator handAnimator;

    private void Awake()
    {
        handAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GripInputAction.action.performed += GripPressed;
        TriggerInputAction.action.performed += TriggerPressed;

    }

    private void TriggerPressed(InputAction.CallbackContext obj)
    {
        handAnimator.SetFloat("Trigger", obj.ReadValue<float>());
      
    }

    private void GripPressed(InputAction.CallbackContext obj)
    {
        handAnimator.SetFloat("Grip", obj.ReadValue<float>());
    }

    private void OnDisable()
    {
        GripInputAction.action.performed -= GripPressed;
        TriggerInputAction.action.performed -= TriggerPressed;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
