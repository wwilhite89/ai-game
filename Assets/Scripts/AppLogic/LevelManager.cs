using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameDB;
using GameDB.SessionData;
using System.Linq;

/// <summary>
/// Singleton class that manages the actions of the Level, including players' turns.
/// </summary>
public class LevelManager : MonoBehaviour {

    public enum Turn { PLAYER, ENEMY }

    public Turn CurrentTurn { get; private set; }
    public GameObject ActiveCharacter { get; private set; } 
	public CharacterController ActiveCharacterCtrl;
    private GameObject[] characters;
	private GameObject[] enemies;
    private GameManager gameManager;

    #region Public Methods

	void Start() {
		gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        this.characters = null;
        this.enemies = null;
        this.CurrentTurn = Turn.PLAYER;
        this.ActiveCharacter = null;
        this.spawnCharacters();
	}

	/*
	TODO (wil) if we want to keep treating AppLogic/ as a library then this can be moved elsewhere,
	but I feel like this logic is convenient to have and I think it belongs in some kind of game manager.
	*/
	void Update() {
		if(ActiveCharacter != null) {
			switch(ActiveCharacterCtrl.character.status) {
				case Character.Status.ATTACKING:
					highlightEnemies (Color.red);
				//TODO (wil) highlight adjacent enemies
					break;
				case Character.Status.READY:
					break;
				case Character.Status.RESTING:
					ActiveCharacterCtrl.Rest();
					break;
				// (wil) this probably isn't necessary
				case Character.Status.DEAD:
					Kill(ActiveCharacter);
					break;
				default:
					break;
			}
		}
	}

    public void Kill(GameObject character)
    {
        bool characterExists = true;

        if (this.enemies.FirstOrDefault(x => x == character) != null)
            this.enemies = this.enemies.Where(x => x != character).ToArray();
        else if (this.characters.FirstOrDefault(x => x == character) != null)
            this.characters = this.characters.Where(x => x != character).ToArray();
        else
            characterExists = false;

        if (characterExists)
            UnityEngine.Object.Destroy(character);
        else
            Debug.LogError("Cannot kill character. Character cannot be found.");

        this.checkGameEnd();
    }

    public void SetActiveCharacter(GameObject character)
    {
        this.ActiveCharacter = character;
		this.ActiveCharacterCtrl = character.GetComponent<CharacterController>();
    }

	public GameObject[] getEnemies() {
		return enemies;
	}

	public GameObject[] getPlayers() {
		return characters;
	}

    public bool IsTurn(GameObject player)
    {
        if (player.tag == GameConstants.TAG_ENEMY)
            return this.CurrentTurn == Turn.ENEMY;
        return this.CurrentTurn == Turn.PLAYER;
    }

    public void CheckTurnEnd()
    {
        var characters = this.CurrentTurn == Turn.ENEMY ? this.enemies : this.characters;
        bool allCharactersActioned = characters.Count(x =>
            {
                var p = x.GetComponent<CharacterController>();
                return !p.HasAttacked || !p.HasMoved;
            }) == 0;

		//if (allCharactersActioned)
			//TODO (wil) highlight end turn button instead
    }
	
    public void ChangeTurn()
    {
        this.CurrentTurn = this.CurrentTurn == Turn.ENEMY ? Turn.PLAYER : Turn.ENEMY;
        var characters = this.CurrentTurn == Turn.ENEMY ? this.enemies : this.characters;
        characters.ToList().ForEach(x => x.GetComponent<CharacterController>().ResetTurn());
    }

    #endregion

    #region Private Methods
    
    private void spawnCharacters()
    {
        int taken = 0;

        // Get enemies to fight
        var enemyChars = gameManager.GetCurrentEnemies();
        this.enemies = GameObject.FindGameObjectsWithTag(GameConstants.TAG_ENEMY);
        this.enemies.ToList().ForEach(x =>
        {
            x.GetComponent<SpawnPointScript>()
                .SpawnCharacter(
                x.GetComponent<CharacterController>(),
                enemyChars.Skip(taken++).FirstOrDefault());
        });

        if (taken < enemyChars.Count)
            Debug.LogWarning(string.Format("Only {0} out of {1} enemy characters were spawned.", taken, enemyChars.Count));

        taken = 0;

        // Get current characters (some might have died last level)
        var playerChars = gameManager.GetCurrentPlayers();
        this.characters = GameObject.FindGameObjectsWithTag(GameConstants.TAG_PLAYER);
        this.characters.ToList().ForEach(x =>
        {
            x.GetComponent<SpawnPointScript>()
                .SpawnCharacter(
                x.GetComponent<CharacterController>(),
                playerChars.Skip(taken++).FirstOrDefault());
        });

        if (taken < playerChars.Count)
            Debug.LogWarning(string.Format("Only {0} out of {1} player characters were spawned.", taken, playerChars.Count));
    }

	private void highlightEnemies(Color c) {
		for (int i = 0; i < ActiveCharacterCtrl.enemiesInRange.Length; i++) {
			ActiveCharacterCtrl.enemiesInRange[i].renderer.material.color = c;
		}
	}

	public void resetCharColor() {
		for (int i = 0; i < characters.Length; i++)
			characters [i].renderer.material.color = Color.grey;
		for (int i = 0; i < enemies.Length; i++)
			enemies [i].renderer.material.color = Color.grey;
	}

    private void checkGameEnd()
    {
        if (this.characters.Count () == 0) {
			gameManager.GameOver ();
			Debug.Log ("Game Over");
		}
        else if (this.enemies.Count() == 0)
        {
            gameManager.GameOver();
            Debug.Log("Level Complete!");
        }
    }

	public void manageAIMove (CharacterController.Decision decision) {
		ActiveCharacterCtrl.MoveAI (decision);
	}
    
    #endregion
}
