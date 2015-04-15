using UnityEngine;
using System.Collections;
using GameDB.SessionData;
using System;


public class LandScript : MonoBehaviour {
	private GameObject player;
	private GameObject enemy;
	private GameObject currentChar;
	private GameObject levelManager;
	public string LandType;
	private Color highlighColor;
	private GameObject[] players;
	private GameObject[] enemies;
	private Vector3 blockPossition;
	private float playerDist;
	private Color landColor;
	public LevelManagerScript.Turn currentTurn;

	// Modifies movement speed of characters. 1 is normal, 0 is unwalkable.
	public float speed;

	// Use this for initialization
	void Start () {
		if (renderer.material.HasProperty ("_Color")) {
			landColor = renderer.material.color;
			highlighColor = new Color (255, renderer.material.color.g, renderer.material.color.b);
		}

		levelManager = GameObject.FindGameObjectWithTag ("Map");
	}
	
	void Awake() {

	}

	// Update is called once per frame
	void Update () {
        currentTurn = GameObject.Find(GameConstants.TAG_MAP).GetComponent<LevelManagerScript>().getTurn();

		player = levelManager.GetComponent<LevelManagerScript> ().getActivePlayer ();
		
		HighlightLand ();
	}
	
	void HighlightLand() {
		float dist;
		
		if (player != null) {
			dist = Vector3.Distance (gameObject.transform.position, player.transform.position);
			
			// set the land color to the default color
			this.renderer.material.color = landColor;
			
			// highlight the land around a selected player if he is active and able to move
			if (dist < player.GetComponent<PlayerScript>().GetSpeed() && gameObject.GetComponent<LandScript>().speed != 0) {
				if (player.GetComponent<PlayerScript>().isActive && player.GetComponent<PlayerScript>().pieceLeftToMove == true)
					this.renderer.material.color = highlighColor;
			}
		}
	}
	
	void OnMouseDown() {
		// Land must be walkable

		player = levelManager.GetComponent<LevelManagerScript> ().getActivePlayer ();

		if(player != null && gameObject.GetComponent<LandScript>().speed != 0) {
			blockPossition = gameObject.transform.position;
			blockPossition.y += .6f;
			
			playerDist = Vector3.Distance (gameObject.transform.position, player.transform.position);
			
			Debug.Log("you clicked distance " + playerDist);
			
			if ( playerDist < player.GetComponent<PlayerScript>().GetSpeed() && player.GetComponent<PlayerScript>().pieceLeftToMove == true ) {

				if (levelManager.GetComponent<LevelManagerScript>().isTurn(player)) {
					player.GetComponent<PlayerScript>().movePlayer(blockPossition);
	                levelManager.GetComponent<LevelManagerScript>().checkTurnEnd();
				}
			}
		}
	}


	// is called every frame on objects that are colliding
	void OnCollisionStay (Collision col){

		if (col.collider.tag == "Player" || col.collider.tag == "Enemy") {
			this.renderer.material.color = landColor;
		}
	}

	void OnMouseOver() {
		
	} 
	
	void OnMouseExit() {
		
	}
}
