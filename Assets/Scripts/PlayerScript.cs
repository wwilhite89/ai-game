using UnityEngine;
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
    private int health;
    private int speed;
    private int defense;
    private int attack;
    private float evasion;
    private float critical;
    private int range;
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
        // Set as active
        this.levelManager.SetActivePlayer(this.gameObject);

        //TODO (wil) This is bad. I need another way to identiy selected character
        gameObject.tag = "Selected";

        levelManager.SetActivePlayer(gameObject);
        enemiesInRange = gameObject.GetComponent<AttackRangeScript>().getObjectsInRadius(opponent);
        enemiesInRange = gameObject.GetComponent<AttackRangeScript>().getObjectsInRadius(opponent);
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

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 60, 30), message))
        {
            Debug.Log("Clicked the button with text");
        }
        if (GUI.Button(new Rect(10, 105, 60, 30), "Continue"))
        {
            Debug.Log("Clicked the button with text");
        }
    }
}
