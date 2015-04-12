using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the actions of the Level, including players' turns.
/// </summary>
public class LevelManagerScript : MonoBehaviour {

    public enum Turn { PLAYER, ENEMY, };

	private GameObject[] players;
	private GameObject[] enemies;
	private Turn currentTurn;

	// Use this for initialization
	void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        currentTurn = Turn.PLAYER;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void resetActive() {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerScript>().isActive = 0;
		}
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<PlayerScript>().isActive = 0;
		}
	}
	public void resetLeftToMove(Turn currentTurn) {
		if (currentTurn == Turn.PLAYER) {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<PlayerScript>().pieceLeftToMove = true;
			}
		}
		else if (currentTurn == Turn.ENEMY) {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<PlayerScript>().pieceLeftToMove = true;
			}
		}
	}

	public void resetPlayers(){
        players = GameObject.FindGameObjectsWithTag("Player");
		enemies = GameObject.FindGameObjectsWithTag ("Enemy");
	}
	public Turn getTurn () {
		return this.currentTurn;
	}
	
	public void changeTurn () {
        if (currentTurn == Turn.PLAYER)
        {
            currentTurn = Turn.ENEMY;
            resetLeftToMove(Turn.ENEMY);
        }
        else if (currentTurn == Turn.ENEMY)
        {
            currentTurn = Turn.PLAYER;
			resetLeftToMove(Turn.PLAYER);
		}
	}
	public void checkTurnEnd(){ 
		bool done=true;
        if (currentTurn == Turn.PLAYER)
        {
			for (int i = 0; i < players.Length; i++) {
                if (players[i].GetComponent<PlayerScript>().pieceLeftToMove == true)
                {
					done = false;
					break;
				} else {
					done = true;
				}
			}
			if (done == true) {
				changeTurn ();
			}
		} else if (currentTurn == Turn.ENEMY) {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].GetComponent<PlayerScript>().pieceLeftToMove == true)
                {
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

	public void attack() {
		if (currentTurn == Turn.ENEMY) 
			Debug.Log ("AI to attack Player");
		else
			Debug.Log ("Player to attack AI");
	}
}
