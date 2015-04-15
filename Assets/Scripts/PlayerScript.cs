using UnityEngine;
using System.Collections;
using GameDB;
using GameDB.SessionData;

public class PlayerScript : MonoBehaviour {

	public bool isActive;
	public bool pieceLeftToMove;
    private Character character;
	public GameObject[] enemiesInRange;
	private GameObject levelManager;
	private GameObject[] Land;
	public string opponent;
    private bool initialized = false;// the text for the button
	public int health;
	private int speed;
	private int defense;
	private int attack;
	private float evasion;
	private float critical;
	private bool setStats = false; // use to set the player stats locally

	// Use this for initialization
	void Start () {
        // Defaul values
		isActive = false;
        pieceLeftToMove = true;
		Land = GameObject.FindGameObjectsWithTag ("Land");

		levelManager = GameObject.FindGameObjectWithTag ("Map");
		// Find out who the opponent is and set the string
		opponent = gameObject.tag == GameConstants.TAG_ENEMY ? GameConstants.TAG_PLAYER : GameConstants.TAG_ENEMY;

	}
	
	// Update is called once per frame
	void Update () {

		if (!setStats) {
			InitializePlayerStats ();
			setStats = true;
		}
	}

	void OnMouseDown() {
		levelManager.GetComponent<LevelManagerScript>().resetActive ();

		// this keeps track of the active player
		isActive = true;

		//TODO (wil) This is bad. I need another way to identiy selected character
		// gameObject.tag = "Selected"; // this breaks a lot of code try using a boolean.

		levelManager.GetComponent<LevelManagerScript>().setActivePlayer(gameObject, true);
		enemiesInRange = gameObject.GetComponent<AttackRangeScript> ().getObjectsInRadius (opponent);
	}

	private void InitializePlayerStats() {
		health = character.health;
		defense = character.defense;
		speed = character.movement;
		attack = character.attack;
	}
	public void movePlayer(Vector3 location) {

		transform.position = location;
		enemiesInRange = gameObject.GetComponent<AttackRangeScript> ().getObjectsInRadius (opponent);

		if (enemiesInRange.Length > 0) {

			Debug.Log("Attacking Player");
			attackPrompt(enemiesInRange[0]);
		}

		isActive = false;

		pieceLeftToMove = false;
	}

	public void attackPrompt(GameObject enemy) {

		enemy.GetComponent<PlayerScript>().health -= this.attack;

		if (enemy.GetComponent<PlayerScript> ().health <= 0) {
			levelManager.GetComponent<LevelManagerScript>().destroyEnemy(enemy);
		}
	}

    public bool IsInitialized()
    {
        return this.initialized;
    }

	public float GetStat(Character.Stats stat) {
		switch (stat) {
		case Character.Stats.ATT:
			return this.character.attack;
		case Character.Stats.CRIT:
			return this.character.critical;
		case Character.Stats.DEF:
			return this.character.defense;
		case Character.Stats.EVA:
			return this.character.evade;
		case Character.Stats.HP:
			return this.character.health;
		case Character.Stats.MOV:
			return this.character.movement;
		case Character.Stats.RANGE:
			return this.character.range;
		default:
			Debug.LogError(string.Format("Could not find appropriate stat for: {0}", stat));
			return -1;
		}
	}

    /// <summary>
    /// Required: IsInitialized() 
    /// </summary>
    /// <returns>Returns the current health of the character.</returns>
    public int GetHealth()
    {
        return this.character.health;
    }

    /// <summary>
    /// Required: IsInitialized() 
    /// </summary>
    /// <returns>Returns the current speed of the character.</returns>
    public int GetSpeed()
    {
        return this.character.movement;
    }

    /// <summary>
    /// Required: IsInitialized() 
    /// </summary>
    /// <returns>Returns the current attack range of the character.</returns>
    public int GetAttackRange()
    {
        return this.character.range;
    }

    /// <summary>
    /// Initializes the script with a character.
    /// </summary>
    /// <param name="c"></param>
    public void SetCharacter(Character c)
    {
        this.character = c;
        this.initialized = true;
    }
}
