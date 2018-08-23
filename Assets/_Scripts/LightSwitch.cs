﻿using System.Collections;
using UnityEngine;

public class LightSwitch : MonoBehaviour {

	public LightRoom room;
	public int id;

	private float initIntensity;
	private new Light light;
	private GameObject bulb;
	private bool rotated = true;

	void Awake () {
		for(int i = 0; i < room.lights.Length; i++){
			Transform temp = room.lights[i];
			int random = Random.Range(0, room.lights.Length);
			room.lights[i] = room.lights[random];
			room.lights[random] = temp;
		}
	}

	void Start () {
		light = room.lights[id].GetChild(0).GetComponent<Light>();
		initIntensity = light.intensity;
		light.intensity = 0;
		bulb = room.lights[id].GetChild(1).gameObject;
		bulb.SetActive(false);
		room.lights[id].GetComponent<LightLamp>().id = id;
		transform.GetChild(1).GetComponent<Renderer>().materials[1].SetTexture("_MainTex", Resources.Load<Texture2D>("Label3_" + (id + 1).ToString()));
	}

	void OnTriggerStay (Collider col) {
		if(!GameManager.solved[3] && col.CompareTag("Player") && rotated && Input.GetButtonDown("Click")){
			light.intensity = initIntensity - light.intensity;
			bulb.SetActive(!bulb.activeSelf);
			StartCoroutine("RotateHandle");
		}
	}

	IEnumerator RotateHandle () {
		rotated = false;
		int dir = (int)((light.intensity / initIntensity - 0.5f) * 2);
		while((dir == 1 && Mathf.Abs(transform.GetChild(0).eulerAngles.z - 70) > 0.1f) || (dir == -1 && Mathf.Abs(transform.GetChild(0).eulerAngles.z - 290) > 0.1f )){
			transform.GetChild(0).Rotate(transform.GetChild(0).forward * -dir * 5);
			yield return null;
		}
		rotated = true;
	}

}
