using UnityEngine;
using System.Collections;
using System;


public class LandScript : MonoBehaviour {
	private GameObject player;
	public string LandType;
	private Color highlighColor;
	private GameObject[] players;
	private Vector3 blockPossition;
	private float playerDist;
	private Color landColor;

	// Modifies movement speed of characters. 1 is normal, 0 is unwalkable.
	public float speed;
	public bool ocupado;

	// Use this for initialization
	void Start () {
		if (renderer.material.HasProperty ("_Color")) {
			landColor = renderer.material.color;
			highlighColor = new Color (255, renderer.material.color.g, renderer.material.color.b);
		}
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
		
		HighlightLand ();
	}
	
	void HighlightLand() {
		float dist;
		
		if (player != null) {
			dist = Vector3.Distance (gameObject.transform.position, player.transform.position);
			
			// set the land color to the default color
			this.renderer.material.color = landColor;
			
			// highlight the land around a selected player if he is active and able to move
			if (dist < player.GetComponent<PlayerScript>().moveSpeed && gameObject.GetComponent<LandScript>().speed != 0) {
				if (player.GetComponent<PlayerScript>().isActive == 1 && player.GetComponent<PlayerScript>().pieceLeftToMove == true)
					this.renderer.material.color = highlighColor;
			}
		}
	}
	
	void OnMouseDown() {
		// Land must be walkable
		if(player != null && gameObject.GetComponent<LandScript>().speed != 0) {
			blockPossition = gameObject.transform.position;
			blockPossition.y += 1;
			
			playerDist = Vector3.Distance (gameObject.transform.position, player.transform.position);
			
			Debug.Log("you clicked distance " + playerDist);
			
			if ( playerDist < player.GetComponent<PlayerScript>().moveSpeed && player.GetComponent<PlayerScript>().pieceLeftToMove == true) {
				player.GetComponent<PlayerScript>().isActive = 0;
				
				player.transform.position = blockPossition;
				
				//player.GetComponent<CameraScript>().(player.transform.position;
				player.GetComponent<PlayerScript>().pieceLeftToMove = false;
			}
		}
	}
	
	void OnMouseOver() {
		
	} 
	
	void OnMouseExit() {
		
	}
}
