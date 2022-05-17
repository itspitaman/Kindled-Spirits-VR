using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    Health campfireHealth;
    public float timeBetweenTicks;
    public int tickDamage;
    float timer;

    public int relightHealth;
    public bool isLit;

    public GameObject flame;
    public GameObject healingWarmth;

    public List<Collider> kindlingColliders;
    public int kindlingHealth;

    CampfireHealthSync _campfireHealthSync;
    AudioSource audioSource;


    private void Awake()
    {
        campfireHealth = GetComponent<Health>();
        _campfireHealthSync = GetComponent<CampfireHealthSync>();
    }



    // Start is called before the first frame update
    void Start()
    {
        CampfireManager.Instance.campfireHealths.Add(campfireHealth);
        campfireHealth.currentHitPoints = 0;
        timer = 0f;
        TurnOffCampfire();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;

        if (timer > timeBetweenTicks)
        {
            timer = 0f;
            campfireHealth.TakeDamage(tickDamage);
        }

        if (campfireHealth.currentHitPoints <= 0)
        {
            TurnOffCampfire();
            audioSource.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Torch"))
        {
            ToastingCollider toastingCollider = other.gameObject.GetComponent<ToastingCollider>();

            if (toastingCollider.GetComponentInParent<Torch>().isLit)
            {
                RelightCampfireWithTorch();
            }
        }

        if (other.gameObject.CompareTag("Kindling"))
        {
            if (isLit)
            {
                campfireHealth.HealDamage(kindlingHealth);
                Destroy(other.gameObject);
            }
            else
            {
                kindlingColliders.Add(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Kindling"))
        {
            kindlingColliders.Remove(other);
        }
    }

    public void TurnOnCampfire()
    {
        isLit = true;
        campfireHealth.isDead = false;
        flame.SetActive(true);
        healingWarmth.SetActive(true);
    }

    public void TurnOffCampfire()
    {
        isLit = false;
        campfireHealth.isDead = true;
        flame.SetActive(false);
        healingWarmth.SetActive(false);
    }

    void RelightCampfireWithTorch()
    {
        if (campfireHealth.currentHitPoints < relightHealth)
        {
            campfireHealth.currentHitPoints = relightHealth;

            if (kindlingColliders.Count > 0)
            {
                foreach (Collider kindling in kindlingColliders)
                {
                    campfireHealth.HealDamage(kindlingHealth);
                    Destroy(kindling.gameObject);
                }
            }

            campfireHealth.UpdateHealthSlider();
            TurnOnCampfire();
            audioSource.Play();
            _campfireHealthSync.UpdateCampfireModelHealth();
        }
    }
}
