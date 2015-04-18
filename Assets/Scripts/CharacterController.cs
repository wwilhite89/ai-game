using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GameDB;
using GameDB.SessionData;

public class CharacterController : MonoBehaviour
{
	//TODO (wil) this might make more sense to be in Database/Character.cs. I'll put it here for now.
	public enum Status {
		READY,
		ATTACKING,
		RESTING,
		DEAD
	}

    private Character character;
    public GameObject[] enemiesInRange;
    private GameObject[] Land;
    public string opponent;
    public bool HasMoved { get; private set; }
    public bool HasAttacked { get; private set; }
	public bool selected;

    private bool initialized = false;
    private string message; // the text for the button
    public int health;
    public int speed;
    public int defense;
    public int attack;
    public float evasion;
    public float critical;
    public int range;
	public Status currentStatus = Status.READY;

    private BattleManager battleMgr;
    private LevelManager levelManager;

    // Use this for initialization
    void Start()
    {
		GameObject manager = GameObject.Find("Manager");
        this.levelManager = manager.GetComponent<LevelManager>();
        this.battleMgr = manager.GetComponent<BattleManager>();

        // Defaul values
        message = "";
        Land = GameObject.FindGameObjectsWithTag("Land");

        // Find out who the opponent is and set the string
        opponent = gameObject.tag == GameConstants.TAG_ENEMY ? GameConstants.TAG_PLAYER : GameConstants.TAG_ENEMY;
    }

    void OnMouseDown()
    {
        if (this.levelManager.IsTurn(this.gameObject))
        {
            // Set as active
            this.levelManager.SetActiveCharacter(this.gameObject);

            enemiesInRange = gameObject.GetComponent<AttackRangeScript>().getObjectsInRadius(opponent);
            enemiesInRange = gameObject.GetComponent<AttackRangeScript>().getObjectsInRadius(opponent);
        }
    }

    public void moveCharacter(Vector3 location)
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

	// Method for being attacked
    public void attackPrompt(GameObject opponent)
    {
        battleMgr.DoBattle(opponent.GetComponent<CharacterController>(), this);
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
            levelManager.SetActiveCharacter(null);
            levelManager.CheckTurnEnd();
        }
    }
}
