using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTurunan : MonoBehaviour
{

    void Start()
    {
        hasEntered = false;
    }

    void Update()
    {
    }

    bool hasEntered;
    void OnTriggerStay(Collider col)
    {
        if (gameObject.activeSelf && col.tag == "Player")
        {
            var cpBound = GetComponent<Collider>().bounds;
            var truckBound = col.bounds;

            if (cpBound.Contains(truckBound.min) && cpBound.Contains(truckBound.max))
            {
                hasEntered = true;
                var rb = col.transform.parent.GetComponentInParent<Rigidbody>();

                if (rb.velocity.magnitude < 0.01f)
                {
                    hasEntered = false;
                    gameObject.SetActive(false);
                    
                }
            }
            else if (hasEntered)
            {
                GetComponentInParent<TanjakanManager>().status = ExamController.Status.Failed;
                hasEntered = false;
            }
            
        }
    }

    void OnTriggerExit(Collider col)
    {
        //hasEntered = false;
    }

}
