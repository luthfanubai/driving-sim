using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller Lampu lalu lintas untuk sisi utara dan selatan
/// </summary>
public class TrafficLightControllerA : MonoBehaviour {

	Shader shaderOn;
	Shader shaderOff;
	Renderer rend;

	//rend.materials[0] -> texture
	//rend.materials[1] -> lampu merah
	//rend.materials[2] -> lampu kuning
	//rend.materials[3] -> lampu hijau

	void Start() {
		rend = GetComponent<Renderer>();
		shaderOn = Shader.Find("Self-Illumin/Diffuse");
		shaderOff = Shader.Find("Diffuse");

		InvokeRepeating("ToggleLights", 0.0f, 5.0f); //method invoke setiap interval 5 detik setelah 0 detik pertama

		TurnOffAllLights(); //pastikan semua lampu mati
		rend.materials[1].shader = shaderOn; //inisiasi lampu merah menyala terlebih dahulu

	}

	void TurnOffAllLights()
	{
		for (int i = 0; i < rend.materials.GetLength (0); i++)
			rend.materials [i].shader = shaderOff;
	}

	void ToggleLights()
	{
		for (int i = 0; i < rend.materials.GetLength (0); i++) {
			if (i != 2 && i != 0) //selama bukan lampu kuning
			{
				if (rend.materials [i].shader == shaderOn) //toggle lampu nyala --> mati, dsb
					rend.materials [i].shader = shaderOff;
				else
					rend.materials [i].shader = shaderOn;
			}
		}

	}
}

