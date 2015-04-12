using UnityEngine;
using System.Collections;
using GameDB;
using GameDB.SessionData;

public class PlayerScript : MonoBehaviour {

	public bool isActive;
	public bool pieceLeftToMove;

    private Character character;
	private GameObject[] enemiesInRange;
	private string opponent;
    private bool initialized = false;

	// Use this for initialization
	void Start () {
        // Defaul values
		isActive = false;
        pieceLeftToMove = true;

		// Find out who the opponent is and set the string
		opponent = gameObject.tag == GameConstants.TAG_ENEMY ? GameConstants.TAG_PLAYER : GameConstants.TAG_ENEMY;
	}
	
	// Update is called once per frame
	void Update () {
		enemiesInRange = gameObject.GetComponent<AttackRangeScript> ().getObjectsInRadius (opponent);
	}

	void OnMouseDown() {

		this.GetComponentInParent<LevelManagerScript>().resetActive ();
		isActive = true;

	}

	void OnTriggerEnter( Collider col ) {

		if (this.gameObject.tag == "Player" && col.gameObject.tag == "Enemy") {
            gameObject.GetComponentInParent<LevelManagerScript>().attack();
		}
		if (this.gameObject.tag == "Enemy" && col.gameObject.tag == "Player") {
            gameObject.GetComponentInParent<LevelManagerScript>().attack();
		}
	}

    public bool IsInitialized()
    {
        return this.initialized;
    }

    /// <summary>
    /// Required: IsInitialized() 
    /// </summary>
    /// <returns>Returns the current health of the character.</returns>
    public int GetHealth()
    {
        return this.character.HP;
    }

    /// <summary>
    /// Required: IsInitialized() 
    /// </summary>
    /// <returns>Returns the current speed of the character.</returns>
    public int GetSpeed()
    {
        return this.character.MOV;
    }

    /// <summary>
    /// Required: IsInitialized() 
    /// </summary>
    /// <returns>Returns the current attack range of the character.</returns>
    public int GetAttackRange()
    {
        return this.character.RANGE;
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
