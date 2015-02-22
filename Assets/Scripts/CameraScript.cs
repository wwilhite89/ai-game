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
	
		if (Input.GetKey(KeyCode.Alpha1)) // change to camera one by clicking 1 on alpha/numeric pad
		{
			camera1.camera.enabled = true;
			camera2.camera.enabled = false;
		}

		if (Input.GetKey(KeyCode.Alpha2)) // change to camera two by clicking 2 on alpha/numeric pad
		{
			camera1.camera.enabled = false;
			camera2.camera.enabled = true;
		}
	}
}
