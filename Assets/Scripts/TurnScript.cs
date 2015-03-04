using UnityEngine;
using System.Collections;

public class TurnScript : MonoBehaviour {

	public enum Turn{ player, AI,};

	private Turn playerTurn;

	// Use this for initialization
	void Start () {
		playerTurn = Turn.player;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Turn getTurn () {
		return playerTurn;
	}

	public void changeTurn () {
		if (playerTurn == Turn.player)
			playerTurn = Turn.AI;
		else if (playerTurn == Turn.AI)
			playerTurn = Turn.player;
	}
}
