using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointParallelParking : MonoBehaviour
{
    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            gameObject.SetActive(false);
        }
    }

}