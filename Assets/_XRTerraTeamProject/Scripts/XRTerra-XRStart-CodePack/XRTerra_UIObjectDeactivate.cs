// using.XRT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class XRTerra_UIObjectDeactivate : MonoBehaviour, IPointerClickHandler

{
    [Header("GameObject to Set Inactive")]
    public GameObject objectToDeactivate;

    [Header("Tag for Object Trigger Interaction")]
    public string objectTag;

    [Header("Time to Deactivate Object")]
    public float seconds;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void ObjectToDeactivate(GameObject objectToDeactivate)
    {
        objectToDeactivate.SetActive(false);
    }

    IEnumerator ObjectToDeactivateWaitTime()
    {
        yield return new WaitForSecondsRealtime(seconds);
        ObjectToDeactivate(objectToDeactivate);
    }

    public void OnPointerClick(PointerEventData data)
    {
        StartCoroutine(ObjectToDeactivateWaitTime());
    }

}
