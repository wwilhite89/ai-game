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
    public GameObject ActivePlayer { get; private set; }

    private static LevelManager instance = null;
	
    private GameObject[] players;
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
        this.players = null;
        this.enemies = null;
        this.CurrentTurn = Turn.PLAYER;
        this.ActivePlayer = null;
        this.spawnCharacters();
    }

    public void Kill(GameObject player)
    {
        bool playerExists = true;

        if (this.enemies.FirstOrDefault(x => x == player) != null)
            this.enemies = this.enemies.Where(x => x != player).ToArray();
        else if (this.players.FirstOrDefault(x => x == player) != null)
            this.players = this.players.Where(x => x != player).ToArray();
        else
            playerExists = false;

        if (playerExists)
            UnityEngine.Object.Destroy(player);
        else
            Debug.LogError("Cannot kill player. Player cannot be found.");

        this.checkGameEnd();
    }

    public void SetActivePlayer(GameObject player)
    {
        this.ActivePlayer = player;
    }

    public bool IsTurn(GameObject player)
    {
        if (player.tag == GameConstants.TAG_ENEMY)
            return this.CurrentTurn == Turn.ENEMY;
        return this.CurrentTurn == Turn.PLAYER;
    }

    public void CheckTurnEnd()
    {
        var players = this.CurrentTurn == Turn.ENEMY ? this.enemies : this.players;
        bool allPlayersActioned = players.Count(x =>
            {
                var p = x.GetComponent<PlayerScript>();
                return !p.HasAttacked || !p.HasMoved;
            }) == 0;


        if (allPlayersActioned)
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
                .SpawnPlayer(
                x.GetComponent<PlayerScript>(),
                enemyChars.Skip(taken++).FirstOrDefault());
        });

        if (taken < enemyChars.Count)
            Debug.LogWarning(string.Format("Only {0} out of {1} enemy characters were spawned.", taken, enemyChars.Count));

        taken = 0;

        // Get current players (some might have died last level)
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

    private void changeTurn()
    {
        this.CurrentTurn = this.CurrentTurn == Turn.ENEMY ? Turn.PLAYER : Turn.ENEMY;
        var players = this.CurrentTurn == Turn.ENEMY ? this.enemies : this.players;
        players.ToList().ForEach(x => x.GetComponent<PlayerScript>().ResetTurn());
    }

    private void checkGameEnd()
    {
        if (this.players.Count() == 0)
            gameManager.GameOver();
        else if (this.enemies.Count() == 0)
        {
            gameManager.GameOver();
            Debug.Log("Level Complete!");
        }
    }
    
    #endregion

}
