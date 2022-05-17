using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTransform : MonoBehaviour
{
    bool isActiveInList;
    bool isInsideWarmthCollider;
    float timer;
    public float timeBetweenChecks = 3f;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > timeBetweenChecks)
        {
            bool closeToPlayer = false;
            foreach(Transform player in EnemyManager.Instance.playerTransforms)
            {
                if((transform.position - player.position).magnitude < EnemyManager.Instance.maxDistanceFromPlayer)
                {
                    closeToPlayer = true;
                }
            }
            if(!isActiveInList && !isInsideWarmthCollider && closeToPlayer )
            {
                AddToList();
            }

            if(isActiveInList && (isInsideWarmthCollider || !closeToPlayer))
            {
                RemoveFromList();
            }
        }
    }

    void AddToList()
    {
        EnemyManager.Instance.spawnTransforms.Add(transform);
        isActiveInList = true;
    }

    void RemoveFromList()
    {
        EnemyManager.Instance.spawnTransforms.Remove(transform);
        isActiveInList = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("WarmthCollider"))
        {
            isInsideWarmthCollider = true;
            RemoveFromList();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("WarmthCollider"))
        {
            isInsideWarmthCollider = false;
          
        }
    }
}
