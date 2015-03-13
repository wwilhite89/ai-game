using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	private int active;
	private Vector3 cameraOffset1;
	private Vector3 cameraOffset2;
	private int currentCam;
	public bool pieceLeftToMove;
	public float moveSpeed;


	// Use this for initialization
	void Start () {
		active = 0;
		pieceLeftToMove = true;
		cameraOffset1 = new Vector3(0,7,-4);
		cameraOffset2 = new Vector3 (0, 10, 0);
		currentCam = 1;
	}
	
	// Update is called once per frame
	void Update () {

		// check to see if the player wants to change cameras
		if (Input.GetKey (KeyCode.Alpha1))
						arialCam ();
		if (Input.GetKey (KeyCode.Alpha2))
						angleCam ();
	}

	void OnMouseDown() {
		active = 1;

		Camera.main.camera.transform.position = cameraPosition (gameObject.transform.position);
	}

	private Vector3 cameraPosition ( Vector3 position ) {
		Vector3 newPosition;

		if (currentCam == 1) {
			newPosition.x = position.x + cameraOffset1.x;
			newPosition.y = position.y + cameraOffset1.y;
			newPosition.z = position.z + cameraOffset1.z;
		}

		else {
			newPosition.x = position.x + cameraOffset2.x;
			newPosition.y = position.y + cameraOffset2.y;
			newPosition.z = position.z + cameraOffset2.z;
		}
		
		return newPosition;
	}

	private void arialCam() {
		if (currentCam == 1) {
			currentCam = 2;
			Camera.main.camera.transform.position = cameraPosition (gameObject.transform.position);
			Camera.main.transform.rotation = Quaternion.Euler(90.0f,0.0f,0.0f);
		}
	}

	private void angleCam() {
		if (currentCam == 2) {
			currentCam = 1;
			Camera.main.camera.transform.position = cameraPosition (gameObject.transform.position);
			Camera.main.transform.rotation = Quaternion.Euler(49.0f,0.0f,0.0f);
		}
	}
}
