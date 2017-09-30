using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script trigger masing-masing patok
/// </summary>
public class PatokTrigger : MonoBehaviour {

    PatokManager patokManager;
	void Start () {
        patokManager = GetComponentInParent<PatokManager>();
	}
    
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Patok")
        {
            patokManager.AddPatokCount();
//            Destroy(this);
        }
    }


}
