using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameDB.SessionData;
using System.Linq;

/// <summary>
/// Singleton class that manages the actions of the Level, including players' turns.
/// </summary>
public class LevelManager {

    public enum Turn { PLAYER, ENEMY, };

    public Turn CurrentTurn { get; private set; }
    public GameObject ActiveCharacter { get; private set; }

    private static LevelManager instance = null;
	
    private GameObject[] characters;
	private GameObject[] enemies;
    private GameManager gameManager = new GameManager();

    #region Public Methods

    public static LevelManager getInstance()
    {
        if (instance == null)
            instance = new LevelManager();
        return instance;
    }

    public void StartNewLevel()
    {
        this.characters = null;
        this.enemies = null;
        this.CurrentTurn = Turn.PLAYER;
        this.ActiveCharacter = null;
        this.spawnCharacters();
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


        if (allCharactersActioned)
            this.changeTurn();

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

    private void changeTurn()
    {
        this.CurrentTurn = this.CurrentTurn == Turn.ENEMY ? Turn.PLAYER : Turn.ENEMY;
        var characters = this.CurrentTurn == Turn.ENEMY ? this.enemies : this.characters;
        characters.ToList().ForEach(x => x.GetComponent<CharacterController>().ResetTurn());
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
    
    #endregion

}
