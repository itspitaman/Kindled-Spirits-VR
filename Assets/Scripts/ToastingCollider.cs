using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastingCollider : MonoBehaviour
{
    Torch torch;
    public int torchDamage;
    public int maxHits;
    int hitsRemaining;

    private void Start()
    {
        torch = GetComponentInParent<Torch>();
        hitsRemaining = maxHits;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Torch") && torch.isLit)
        {
            Torch otherTorch = other.gameObject.GetComponentInParent<Torch>();
            ToastingCollider toastingCollider = other.gameObject.GetComponentInParent<ToastingCollider>();

            if (!otherTorch.isLit)
            {
                otherTorch.TurnOnTorch();
                toastingCollider.hitsRemaining = toastingCollider.maxHits;

            }
        }




        if (other.gameObject.CompareTag("Marshmallow") && torch.isLit)
        {
            other.gameObject.GetComponent<Marshmallow>().isToasting = true;
        }

        if (other.gameObject.CompareTag("Enemy") && torch.isLit)
        {
            Health collidedHealth = other.gameObject.GetComponent<Health>();
            collidedHealth.TakeDamage(torchDamage);
            hitsRemaining--;

            if (hitsRemaining <= 0)
            {
                torch.TurnOffTorch();
            }
        }

        if (other.gameObject.CompareTag("Campfire"))
        {
            SphereCollider campfireCollider = other.gameObject.GetComponent<SphereCollider>();

            if (campfireCollider.GetComponent<Campfire>().isLit && !torch.isLit)
            {
                torch.TurnOnTorch();
                hitsRemaining = maxHits;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Marshmallow"))
        {
            other.gameObject.GetComponent<Marshmallow>().isToasting = false;
        }
    }
}
