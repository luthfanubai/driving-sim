using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Digunakan untuk mengendalikan camera dengan control
/// first-person untuk mengamati environment
/// </summary>
public class CameraController : MonoBehaviour {
	CharacterController characterController;
	[SerializeField]
	float movementSpeed = 1.0f;
	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
		float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;
		Vector3 speed = new Vector3 (sideSpeed, Physics.gravity.y, forwardSpeed);
		characterController.SimpleMove (speed);
	}
}
