using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameDB.SessionData;
using System.Linq;

/// <summary>
/// Manages the actions of the Level, including players' turns.
/// </summary>
public class LevelManagerScript : MonoBehaviour {

    public enum Turn { PLAYER, ENEMY, };

	private GameObject[] players;
	private GameObject[] enemies;
	private Turn currentTurn;
	private GameObject activePlayer;
    private GameManager gameManager = new GameManager();

	// Use this for initialization
	void Start () {
        this.spawnCharacters();
        this.currentTurn = Turn.PLAYER;
		activePlayer = null;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void resetActive() {

		resetPlayers ();

		if (activePlayer != null) { 
			for (int i = 0; i < players.Length; i++) {
				players [i].GetComponent<PlayerScript> ().isActive = false;
			}
			for (int i = 0; i < enemies.Length; i++) {
				enemies [i].GetComponent<PlayerScript> ().isActive = false;
			}
		}
	}

	public void destroyEnemy (GameObject enemy) {
		Destroy (enemy);
		enemies = GameObject.FindGameObjectsWithTag ("Enemy");
	}

	public GameObject getActivePlayer () {
		return activePlayer;
	}

	public void setActivePlayer(GameObject actv, bool b) {
		activePlayer = actv;

		if (activePlayer != null)
			activePlayer.GetComponent<PlayerScript>().isActive = b;
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

    private void spawnCharacters()
    {
        int taken = 0;

        // Get enemies to fight
        var enemyChars = gameManager.GetCurrentEnemies();
        this.enemies = GameObject.FindGameObjectsWithTag(GameConstants.TAG_ENEMY);
        this.enemies.ToList().ForEach(x =>
        {
            x.GetComponent<SpawnPointScript>()
                .SpawnPlayer(
                x.GetComponent<PlayerScript>(),
                enemyChars.Skip(taken++).FirstOrDefault());
        });

        if (taken < enemyChars.Count)
            Debug.LogWarning(string.Format("Only {0} out of {1} enemy characters were spawned.", taken, enemyChars.Count));

        taken = 0;

        // Get current players (some might have dies last level)
        var playerChars = gameManager.GetCurrentPlayers();
        this.players = GameObject.FindGameObjectsWithTag(GameConstants.TAG_PLAYER);
        this.players.ToList().ForEach(x =>
        {
            x.GetComponent<SpawnPointScript>()
                .SpawnPlayer(
                x.GetComponent<PlayerScript>(),
                playerChars.Skip(taken++).FirstOrDefault());
        });

        if (taken < playerChars.Count)
            Debug.LogWarning(string.Format("Only {0} out of {1} player characters were spawned.", taken, playerChars.Count));
    }
}
