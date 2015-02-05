using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public float moveSpeed;
	private Vector3 moveDirection;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		// 1
		Vector3 currentPosition = transform.position;
		// 2
		if( Input.GetButton("Fire1") ) {
			// 3
			Vector3 moveToward = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			// 4
			moveDirection = moveToward - currentPosition;
			moveDirection.z = 0; 
			moveDirection.Normalize();
		}

		Vector3 target = moveDirection * moveSpeed + currentPosition;
		transform.position = Vector3.Lerp( currentPosition, target, Time.deltaTime );
	}
}
