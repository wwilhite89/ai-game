﻿using UnityEngine;
using System.Collections;
using GameDB;
using GameDB.SessionData;

public class PlayerScript : MonoBehaviour
{
    private Character character;
    public GameObject[] enemiesInRange;
    private GameObject[] Land;
    public string opponent;
    public bool HasMoved { get; private set; }
    public bool HasAttacked { get; private set; }

    private bool initialized = false;
    private string message; // the text for the button
    public int health;
    public int speed;
    public int defense;
    public int attack;
    public float evasion;
    public float critical;
    public int range;
    private BattleManager battleMgr;
    private LevelManager levelManager;

    // Use this for initialization
    void Start()
    {
        this.levelManager = LevelManager.getInstance();
        this.battleMgr = new BattleManager();

        // Defaul values
        message = "";
        Land = GameObject.FindGameObjectsWithTag("Land");

        // Find out who the opponent is and set the string
        opponent = gameObject.tag == GameConstants.TAG_ENEMY ? GameConstants.TAG_PLAYER : GameConstants.TAG_ENEMY;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        if (this.levelManager.IsTurn(this.gameObject))
        {
            // Set as active
            this.levelManager.SetActivePlayer(this.gameObject);

            enemiesInRange = gameObject.GetComponent<AttackRangeScript>().getObjectsInRadius(opponent);
            enemiesInRange = gameObject.GetComponent<AttackRangeScript>().getObjectsInRadius(opponent);
        }
    }

    public void movePlayer(Vector3 location)
    {
        transform.position = location;
        enemiesInRange = gameObject.GetComponent<AttackRangeScript>().getObjectsInRadius(opponent);

        if (enemiesInRange.Length > 0)
        {
            message = "Attack";
            attackPrompt(enemiesInRange[0]);
        }
        else
            this.HasAttacked = true;

        message = "";
        this.HasMoved = true;
        this.checkPlayerTurnEnd();
    }

    public void attackPrompt(GameObject enemy)
    {
        battleMgr.DoBattle(this, enemy.GetComponent<PlayerScript>());
        this.HasAttacked = true;
        this.checkPlayerTurnEnd();
    }

    public bool IsInitialized()
    {
        return this.initialized;
    }

    public float GetStat(Character.Stats stat)
    {
        if (!this.initialized)
        {
            Debug.LogError("Character not initialized yet.");
            return -1;
        }

        switch (stat)
        {
            case Character.Stats.ATT:
                return this.attack;
            case Character.Stats.CRIT:
                return this.critical;
            case Character.Stats.DEF:
                return this.defense;
            case Character.Stats.EVA:
                return this.evasion;
            case Character.Stats.HP:
                return this.health;
            case Character.Stats.MOV:
                return this.speed;
            case Character.Stats.RANGE:
                return this.range;
            default:
                Debug.LogError(string.Format("Could not find appropriate stat for: {0}", stat));
                return -1;
        }
    }

    public void UpdateStat(Character.Stats stat, float value)
    {
        if (!this.initialized)
            Debug.LogError("Character not initialized yet.");

        switch (stat)
        {
            case Character.Stats.ATT:
                this.attack = (int)value;
                break;
            case Character.Stats.CRIT:
                this.critical = value;
                break;
            case Character.Stats.DEF:
                this.defense = (int)value;
                break;
            case Character.Stats.EVA:
                this.evasion = value;
                break;
            case Character.Stats.HP:
                this.health = (int)value;
                break;
            case Character.Stats.MOV:
                this.speed = (int)value;
                break;
            case Character.Stats.RANGE:
                this.range = (int)value;
                break;
            default:
                Debug.LogError(string.Format("Could not find appropriate stat for: {0}", stat));
                break;
        }
    }

    /// <summary>
    /// Initializes the script with a character.
    /// </summary>
    /// <param name="c"></param>
    public void SetCharacter(Character c)
    {
        this.character = c;
        health = c.health;
        defense = c.defense;
        attack = c.attack;
        evasion = c.evade;
        speed = c.movement;
        range = c.range;
        critical = c.critical;
        this.initialized = true;
    }

    public void ResetTurn()
    {
        this.HasMoved = this.HasAttacked = false;
    }

    private void checkPlayerTurnEnd()
    {
        if (this.HasAttacked && this.HasMoved)
        {
            levelManager.SetActivePlayer(null);
            levelManager.CheckTurnEnd();
        }
    }
}
