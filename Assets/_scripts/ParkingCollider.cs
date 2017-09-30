using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ParkingCollider : MonoBehaviour {
	[SerializeField]
	GameObject countTextGO;	
	Text countText;
	Rigidbody rbCar;


	void Start () {
		countText = countTextGO.GetComponent<Text> ();
	}

	void Update () {
		
	}

	bool canStartCouroutine = true;
	void OnTriggerStay (Collider col)
	{

		Debug.Log(col.tag);
		if (col.GetComponentInParent<Rigidbody> () != null)
			rbCar = col.GetComponentInParent<Rigidbody> ();

		Debug.Log (rbCar.velocity.magnitude);
		if(rbCar.velocity.magnitude < 1 && canStartCouroutine) {
			StartCoroutine ("Countdown");
			canStartCouroutine = false;
		}
	}
		
	IEnumerator Countdown()
	{		
		countTextGO.SetActive (true);

		if (rbCar.velocity.magnitude > 1) {
			countTextGO.SetActive (false);
			countText.text = "3";
			canStartCouroutine = true;
			yield break;

		}

		yield return new WaitForSeconds (1);
		countText.text = "2";

		if (rbCar.velocity.magnitude > 1) {
			countTextGO.SetActive (false);
			countText.text = "3";
			canStartCouroutine = true;
			yield break;

		}

		yield return new WaitForSeconds (1);
		countText.text = "1";

		if (rbCar.velocity.magnitude > 1) {
			countTextGO.SetActive (false);
			countText.text = "3";
			canStartCouroutine = true;
			yield break;

		}
			
		yield return new WaitForSeconds (1);

		if (rbCar.velocity.magnitude > 1) {
			countTextGO.SetActive (false);
			countText.text = "3";
			canStartCouroutine = true;
			yield break;

		}

		Debug.Break ();

	}
		
}