using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

/// <summary>
/// Menampilkan speedometer & geartype dalam bentuk text / string
/// pada UI
/// </summary>
public class Speedometer : MonoBehaviour {

	[SerializeField] Text speedText;
    [SerializeField] Text gearText;
    Rigidbody rb;
    TruckController tc;
	void Start () {
        rb = GetComponent<Rigidbody>();
        tc = GetComponent<TruckController>();
	}
        

	void Update () {
        speedText.text = string.Format("Kecepatan : {0:F1} km/h", rb.velocity.magnitude * 3.6f); //speed in KM/H
        gearText.text = string.Format("Gear : {0}", tc.GearType);
	}

}
