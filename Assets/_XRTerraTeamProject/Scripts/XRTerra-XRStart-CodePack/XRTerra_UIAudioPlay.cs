// using.XRT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class XRTerra_UIAudioPlay : MonoBehaviour, IPointerClickHandler
{
    [Header("GameObject With Audio Source")]
    public AudioSource audioSource;

    [Header("Tag for Object Trigger Interaction")]
    public string objectTag;

    [Header("Time to Audio Play")]
    public float seconds;

    // Keep start in your hierarchy to enable or disable this script through the inspector and animation windows 
    void Start()
    {
    }

    public void AudioSourcePlay(AudioSource audioSource)
    {
        audioSource.Play();
    }

    IEnumerator AudioSourcePlayWaitTime()
    {
        yield return new WaitForSecondsRealtime(seconds);
        AudioSourcePlay(audioSource);
    }

    public void OnPointerClick(PointerEventData data)
    {
        StartCoroutine(AudioSourcePlayWaitTime());
    }

}
