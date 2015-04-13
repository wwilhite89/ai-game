﻿using UnityEngine;
using System.Collections;
using GameDB;
using GameDB.SessionData;

public class PlayerScript : MonoBehaviour {

	public bool isActive;
	public bool pieceLeftToMove;
    private Character character;
	public GameObject[] enemiesInRange;
	private GameObject levelManager;
	public string opponent;
    private bool initialized = false;
	private string message; // the text for the button

	// Use this for initialization
	void Start () {
        // Defaul values
		isActive = false;
        pieceLeftToMove = true;
		message = "";

		levelManager = GameObject.FindGameObjectWithTag ("Map");
		// Find out who the opponent is and set the string
		opponent = gameObject.tag == GameConstants.TAG_ENEMY ? GameConstants.TAG_PLAYER : GameConstants.TAG_ENEMY;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown() {

		levelManager.GetComponent<LevelManagerScript>().resetActive ();
		isActive = true;

		enemiesInRange = gameObject.GetComponent<AttackRangeScript> ().getObjectsInRadius (opponent);

	}

	public void movePlayer(Vector3 location) {

		transform.position = location;
		enemiesInRange = gameObject.GetComponent<AttackRangeScript> ().getObjectsInRadius (opponent);

		if (enemiesInRange.Length > 0) {
			message = "Attack";
			attackPrompt(enemiesInRange[0]);
		}

		isActive = false;
		pieceLeftToMove = false;
		message = "";
	}

	public void attackPrompt(GameObject enemy) {

	
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

	void OnGUI() {

		if (GUI.Button (new Rect (10, 70, 60, 30), message)) {
			Debug.Log ("Clicked the button with text");
		}
		if (GUI.Button (new Rect (10, 105, 60, 30), "Continue")) {
			Debug.Log ("Clicked the button with text");
		}
	}
}
