using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private GameObject cam;
	public float positionDelta = 0.5f;
	public float yMax = 20.0f;
	public float yMin = 4.0f;
	public KeyCode up = KeyCode.W;
	public KeyCode down = KeyCode.S;
	public KeyCode left = KeyCode.A;
	public KeyCode right = KeyCode.D;

	void Start () {
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	
	void Update () {
		if(Input.GetKey(up))
			cam.transform.position += new Vector3(0,0,positionDelta);
		if(Input.GetKey(down))
			cam.transform.position += new Vector3(0,0,-positionDelta);
		if(Input.GetKey(left))
			cam.transform.position += new Vector3(-positionDelta,0,0);
		if(Input.GetKey(right))
			cam.transform.position += new Vector3(positionDelta,0,0);

		// Zoom with scroll wheel
		if(Input.GetAxis("Mouse ScrollWheel") > 0) {
			if(cam.transform.position.y > yMin) {
				cam.transform.position += new Vector3(0,-positionDelta,0);
			}
		}
		if(Input.GetAxis("Mouse ScrollWheel") < 0) {
			if(cam.transform.position.y < yMax) {
				cam.transform.position += new Vector3(0,positionDelta,0);
			}
		}
	}
	
	void OnMouseDown() {
		cam.transform.position = gameObject.transform.position;
	}

	public void Center(Vector3 pos) {
		pos.y = cam.transform.position.y;
		cam.transform.position = pos;
	}
}
