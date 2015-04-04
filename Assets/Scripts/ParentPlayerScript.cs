using UnityEngine;
using System.Collections;

public class ParentPlayerScript : MonoBehaviour {

	public GameObject[] humans;
	public GameObject[] ai;
	public enum Turn{ player, AI,};
	public Turn playerTurn;
	


	// Use this for initialization
	void Start () {

		humans = GameObject.FindGameObjectsWithTag ("Player");
		ai = GameObject.FindGameObjectsWithTag ("Enemy");
		playerTurn = Turn.player;

	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void resetActive() {
		for (int i = 0; i < humans.Length; i++) {
			humans [i].GetComponent<PlayerScript> ().isActive = 0;
		}
		for (int i = 0; i < humans.Length; i++) {
			ai [i].GetComponent<PlayerScript> ().isActive = 0;
		}
	}
	public void resetLeftToMove(Turn currentTurn) {
		if (currentTurn == Turn.player) {
			for (int i = 0; i < humans.Length; i++) {
				humans [i].GetComponent<PlayerScript> ().pieceLeftToMove = true;
			}
		}
		else if (currentTurn == Turn.AI) {
			for (int i = 0; i < ai.Length; i++) {
				ai [i].GetComponent<PlayerScript> ().pieceLeftToMove = true;
			}
		}
	}

	public void resetPlayers(){
		humans = GameObject.FindGameObjectsWithTag ("Player");
		ai = GameObject.FindGameObjectsWithTag ("Enemy");
	}
	public Turn getTurn () {
		return playerTurn;
	}
	
	public void changeTurn () {
		if (playerTurn == Turn.player) {
			playerTurn = Turn.AI;
			resetLeftToMove(Turn.AI);
		} else if (playerTurn == Turn.AI) {
			playerTurn = Turn.player;
			resetLeftToMove(Turn.player);
		}
	}
	public void checkTurnEnd(){ 
		bool done=true;
		if (playerTurn == Turn.player) {
			for (int i = 0; i < humans.Length; i++) {
				if (humans [i].GetComponent<PlayerScript> ().pieceLeftToMove == true) {
					done = false;
					break;
				} else {
					done = true;
				}
			}
			if (done == true) {
				changeTurn ();
			}
		} else if (playerTurn == Turn.AI) {
			for (int i = 0; i < ai.Length; i++) {
				if (ai [i].GetComponent<PlayerScript> ().pieceLeftToMove == true) {
					done = false;
					break;
				} else {
					done = true;
				}
			}
			if (done == true) {
				changeTurn ();
			}
		}

	}
}
