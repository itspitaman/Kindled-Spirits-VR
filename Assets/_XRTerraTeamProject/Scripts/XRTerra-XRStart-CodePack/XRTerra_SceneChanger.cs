// using.XRT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class XRTerra_SceneChanger : MonoBehaviour, IPointerClickHandler
{
    [Header("Build Number Associated With Scene")]
    public int sceneNumber;

    [ Header ("Tag for Object Trigger Interaction") ]
    public string objectTag;

    [Header("Time to Load Scene")]
    public float seconds;

    // Keep start in your hierarchy to enable or disable this script through the inspector and animation windows 
    void Start()
    {
    }

    public void SceneToChange(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    IEnumerator SceneLoadWaitTime()
    {
        yield return new WaitForSecondsRealtime(seconds);
        SceneToChange(sceneNumber);
    }

    public void OnPointerClick(PointerEventData data)
    {
        StartCoroutine(SceneLoadWaitTime());
    }

}
