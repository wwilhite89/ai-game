using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GameDB;
using GameDB.SessionData;

public class CharacterController : MonoBehaviour
{
    public Character character {get; set;}
    public GameObject[] enemiesInRange;
    private GameObject[] Land;
    public string opponent;
    public bool HasMoved { get; private set; }
    public bool HasAttacked { get; private set; }
	public bool selected;

    private bool initialized = false;

    private BattleManager battleMgr;
    private LevelManager levelManager;

    // Use this for initialization
    void Start()
    {
		GameObject manager = GameObject.Find("Manager");
        this.levelManager = manager.GetComponent<LevelManager>();
        this.battleMgr = manager.GetComponent<BattleManager>();
		
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
		} else if(!gameObject.Equals(levelManager.ActiveCharacter) &&
		          levelManager.ActiveCharacterCtrl.character.status == Character.Status.ATTACKING &&
		          !levelManager.ActiveCharacterCtrl.HasAttacked &&
		          this.character.HouseId != levelManager.ActiveCharacterCtrl.character.HouseId) {
			attackPrompt(levelManager.ActiveCharacter);
			levelManager.ActiveCharacterCtrl.HasAttacked = true;
			levelManager.ActiveCharacterCtrl.character.status = Character.Status.READY;
		}
    }

    public void moveCharacter(Vector3 location)
    {
        transform.position = location;
        enemiesInRange = gameObject.GetComponent<AttackRangeScript>().getObjectsInRadius(opponent);

        if (enemiesInRange.Length > 0)
        {
            attackPrompt(enemiesInRange[0]);
        }
        else
            this.HasAttacked = true;

        this.HasMoved = true;
        this.checkPlayerTurnEnd();
    }

	// Method for being attacked
    private void attackPrompt(GameObject opponent)
    {
        battleMgr.DoBattle(opponent.GetComponent<CharacterController>(), this);
    }

	public void Rest() {
		//TODO (wil) just a placeholder for now
		if(!this.HasAttacked) {
			character.health += 2;

			this.HasAttacked = true;
			this.character.status = Character.Status.READY;
		}
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
                this.character.attack = (int)value;
                break;
            case Character.Stats.CRIT:
                this.character.critical = value;
                break;
            case Character.Stats.DEF:
                this.character.defense = (int)value;
                break;
            case Character.Stats.EVA:
                this.character.evade = value;
                break;
            case Character.Stats.HP:
                this.character.health = (int)value;
                break;
            case Character.Stats.MOV:
                this.character.movement = (int)value;
                break;
            case Character.Stats.RANGE:
                this.character.range = (int)value;
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
