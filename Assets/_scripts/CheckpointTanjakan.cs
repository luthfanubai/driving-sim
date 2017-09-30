using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckpointTanjakan : MonoBehaviour
{
    TanjakanManager tm;
    float backwardDistance = 2.0f;
    public bool hasStopped;
    void Start()
    {
        hasStopped = false;
        tm = GetComponentInParent<TanjakanManager>();
    }

    void Update()
    {
    }

    IEnumerator OnTriggerStay(Collider col)
    {
        if (gameObject.activeSelf && col.tag == "Player")
        {
            var cpBound = GetComponent<Collider>().bounds;
            var truckBound = col.bounds;
            if (cpBound.Contains(truckBound.min) && cpBound.Contains(truckBound.max))
            {

                var rb = col.transform.parent.GetComponentInParent<Rigidbody>();

                if (rb.velocity.magnitude < 0.01f)
                {
                    float zPos = col.transform.position.z - backwardDistance;
                    hasStopped = true;
                    tm.SetObjectiveText("Objective : Naik Ke Atas Tanjakan tanpa boleh mundur");
                    while (true)
                    {
                        if (col.transform.position.z <= zPos)
                        {
                            GetComponentInParent<TanjakanManager>().status = ExamController.Status.Failed;
                            yield break;
                        }
                        else yield return new WaitForFixedUpdate();
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (hasStopped)
            gameObject.SetActive(false);
        else
        {
            GetComponentInParent<TanjakanManager>().status = ExamController.Status.Failed;
        }
    }

}
