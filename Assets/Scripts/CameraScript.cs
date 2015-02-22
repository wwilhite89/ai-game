using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public GameObject camera1;
	public GameObject camera2;

	// Use this for initialization
	void Start () {
		camera2.camera.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKey(KeyCode.Alpha1))
		{
			camera1.camera.enabled = true;
			camera2.camera.enabled = false;
		}

		if (Input.GetKey(KeyCode.Alpha2))
		{
			camera1.camera.enabled = false;
			camera2.camera.enabled = true;
		}
	}
}
