using UnityEngine;
using System.Collections;
using System;

public class MouseClick : MonoBehaviour {

	Transform player;
	private Vector3 blockPossition;
	private float playerDist;
	private float mouseDist;
	private Color landColor;

	// Use this for initialization
	void Start () {
		landColor = renderer.material.color;
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

			if ( playerDist < player.GetComponent<PlayerScript>().moveSpeed )
				player.position = blockPossition;
		}
	}

	void OnMouseOver() {

		mouseDist = Vector3.Distance (gameObject.transform.position, player.position);

		if ( mouseDist < player.GetComponent<PlayerScript>().moveSpeed )
			renderer.material.color = new Color(255, renderer.material.color.g, renderer.material.color.b);
	} 
	
	void OnMouseExit ()
	{
		renderer.material.color = landColor;
	}
}
