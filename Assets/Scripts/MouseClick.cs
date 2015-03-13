using UnityEngine;
using System.Collections;
using System;

public class MouseClick : MonoBehaviour {

	GameObject player;
	private Vector3 blockPossition;
	private float playerDist;
	private float mouseDist;
	private Color landColor;
	private GameObject[] players;

	// Use this for initialization
	void Start () {
		landColor = renderer.material.color;
		players = GameObject.FindGameObjectsWithTag ("Player");

		Debug.Log ("player length" + players.Length);
	}

	void Awake() {
		//player = GameObject.FindGameObjectWithTag ("Player").transform;
		//players = GameObject.FindGameObjectsWithTag ("player");
	}

	// Update is called once per frame
	void Update () {
		for (int i = 0; i < players.Length; i++) {
			if (players [i].GetComponent<PlayerScript> ().isActive == 1)
					player = players [i];
		}
	}

	void OnMouseDown() {
		// Land must be walkable
		if(gameObject.GetComponent<LandScript>().speed != 0) {
			blockPossition = gameObject.transform.position;
			blockPossition.y += 1;

			playerDist = Vector3.Distance (gameObject.transform.position, player.transform.position);

			Debug.Log("you clicked distance " + playerDist);

			if ( playerDist < player.GetComponent<PlayerScript>().moveSpeed && player.GetComponent<PlayerScript>().pieceLeftToMove == true) {
				player.transform.position = blockPossition;
				player.GetComponent<PlayerScript>().isActive = 0;
				Camera.main.transform.position = player.GetComponent<PlayerScript>().cameraPosition(player.transform.position);
				player.GetComponent<PlayerScript>().pieceLeftToMove = false;
			}
		}
	}

	void OnMouseOver() {

		if (player != null) {
			if (player.GetComponent<PlayerScript> ().pieceLeftToMove == true) {
				mouseDist = Vector3.Distance (gameObject.transform.position, player.transform.position);

				if (mouseDist < player.GetComponent<PlayerScript> ().moveSpeed)
					renderer.material.color = new Color (255, renderer.material.color.g, renderer.material.color.b);
			}
		}
	} 
	
	void OnMouseExit ()
	{
		renderer.material.color = landColor;
	}
}
