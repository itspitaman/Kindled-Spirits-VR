using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToPlayerList : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EnemyManager.Instance.playerTransforms.Add(transform);
    }


}
