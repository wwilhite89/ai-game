using UnityEngine;
using System.Collections;

public class AIChoiceScript : MonoBehaviour {

	public enum Decision {ATTCLOSE,ATTWEAK,RUN}
	private GameObject levelManager;

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
		GameObject[] enemies = levelManager.GetComponent<LevelManager>(). getEnemies();
		
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

			this.GetComponent<CharacterController>(). Move (enemy.transform.position);
			break;
		case Decision.ATTWEAK:
			break;
		case Decision.RUN:
			break;
		}
		
	}
}
