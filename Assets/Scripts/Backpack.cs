using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Backpack : MonoBehaviour
{
    public static Backpack Instance;

    public enum GrabMode { Marshmallow, Kindling, Key }
    public GrabMode currentGrabmode;

    public int marshmallowCount;
    public int kindlingCount;
    public int keyCount;

    public GameObject kindlingSelectBackground;
    public GameObject marshmallowSelectBackground;
    public GameObject keySelectBackground;
    public GameObject keyLogoUI;

    public Text marshmallowText;
    public Text kindlingText;
    public Text keyText;
    AudioSource BackPackSounds;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        BackPackSounds = GetComponent<AudioSource>();
        marshmallowCount = 0;
        SetGrabModeMarshmallow();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Marshmallow"))
        {
            marshmallowCount++;
            UpdadeUIText();
            Debug.Log("Added a marshmallow. Current count is: " + marshmallowCount);
            Destroy(other.gameObject);
            BackPackSounds.Play();
        }
        else if (other.gameObject.CompareTag("Kindling"))
        {
            kindlingCount++;
            UpdadeUIText();
            Debug.Log("Added some kindling. Current count is: " + kindlingCount);
            Destroy(other.gameObject);
            BackPackSounds.Play();
        }
        else if (other.gameObject.CompareTag("Key"))
        {
            keyCount++;
            UpdadeUIText();
            if (keyCount >= 1) keyLogoUI.SetActive(true);
            Debug.Log("Added a Key. Current count is: " + keyCount);
            Destroy(other.gameObject);
            BackPackSounds.Play();
        }
        else if (other.gameObject.CompareTag("Controller"))
        {
            other.gameObject.GetComponent<Grabber>().inBackpack = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Controller"))
        {
            other.gameObject.GetComponent<Grabber>().inBackpack = false;
        }
    }

    public void SetGrabModeMarshmallow()
    {
        currentGrabmode = GrabMode.Marshmallow;
        marshmallowSelectBackground.SetActive(true);
        kindlingSelectBackground.SetActive(false);
        keySelectBackground.SetActive(false);
    }

    public void SetGrabModeKindling()
    {
        currentGrabmode = GrabMode.Kindling;
        marshmallowSelectBackground.SetActive(false);
        kindlingSelectBackground.SetActive(true);
        keySelectBackground.SetActive(false);
    }

    public void SetGrabModeKey()
    {
        currentGrabmode = GrabMode.Key;
        marshmallowSelectBackground.SetActive(false);
        kindlingSelectBackground.SetActive(false);
        keySelectBackground.SetActive(true);
    }

    public void UpdadeUIText()
    {
        marshmallowText.text = $"{marshmallowCount}";
        kindlingText.text = $"{kindlingCount}";
        keyText.text = $"{keyCount}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
