using UnityEngine;
using System.Collections;
using System;

public class MouseClick : MonoBehaviour {

	Transform player;
	private Vector3 blockPossition;
	private float playerDist;

	// Use this for initialization
	void Start () {
	
	}

	void Awake() {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		// Land must be walkable
		if(gameObject.GetComponent<LandScript>().speed != 0) {
			blockPossition = gameObject.transform.position;
			blockPossition.y += 1;

			playerDist = Vector3.Distance (gameObject.transform.position, player.position);

			Debug.Log("you clicked distance " + playerDist);
			player.position = blockPossition;
		}
	}

	void OnMouseOver() {

	}
}
