using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeMenuDelay : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject welcome;
    public float delay;

    private void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(delay);
        mainMenu.SetActive(true);
        welcome.SetActive(false);
    }
}
