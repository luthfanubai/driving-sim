using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
}
