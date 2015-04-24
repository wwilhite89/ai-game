using UnityEngine;
using System.Collections;
using GameDB;

public class AIChoiceScript : MonoBehaviour {

	public enum Decision {ATTCLOSE,ATTWEAK,RUN}
	private GameObject levelManager;
	public GameObject[] walkable;
	public GameObject[] enemies;

	// Use this for initialization
	void Start () {
		levelManager = GameObject.FindGameObjectWithTag ("GameController");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MoveAI ( Decision decision ) {
		GameObject enemy = null;
		float dist = 0.0f;
		float newDist = 100.0f;
	
		enemies = this.GetComponent<CharacterController>().enemies;
		walkable = this.GetComponent<CharacterController> ().getWalkableLand ();
		
		switch(decision) {
		case Decision.ATTCLOSE:
			// look through the enemies in range and see if any are in reach
			for (int i = 0; i < enemies.Length; i++) {
				dist = Vector3.Distance(this.transform.position, enemies[i].transform.position);
				if (dist < newDist) {
					newDist = dist;
					enemy = enemies[i];
				}
			}

			this.GetComponent<CharacterController>(). Move (walkTo(enemy));		
			break;
		case Decision.ATTWEAK:
			break;
		case Decision.RUN:
			break;
		}
		
	}

	Vector3 walkTo(GameObject enemy) {
		float dist;
		float newDist = 1000.0f;
		GameObject Land = null;

		// what walkable land is closest to the enemy
		for (int i = 0; i < walkable.Length; i++) {
			dist = Vector3.Distance(enemy.transform.position, walkable[i].transform.position);
			if (dist < newDist) {
				newDist = dist;
				Land = walkable[i];
			}
		}
		if (Land != null)
			return Land.transform.position;

		return this.transform.position;
	}
}
