using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GameDB;
using GameDB.SessionData;
using System.Linq;
using ArtificialNeuralNetworks.Core;
using ArtificialNeuralNetworks.Training;

public class CharacterController : MonoBehaviour
{
    public Character character {get; set;}
    public GameObject[] enemiesInRange;
	public GameObject[] enemies;
    private GameObject[] Land;
	public GameObject[] walkableLand;
    public string opponent;
    public bool HasMoved { get; private set; }
    public bool HasAttacked { get; private set; }
	public bool selected;
    private bool initialized = false;
    private BattleManager battleMgr;
    private LevelManager levelManager;
	private GameObject attackButton;
	public Vector3 startPos;
    private bool isAttacking = false;

    private bool isTraining = false;
    private INetworkTrainer trainer;
    private INeuralNetwork attackNetwork;
    private bool hasNetworkAttached;

    private int HP;

    // Use this for initialization
    void Start()
    {
		GameObject manager = GameObject.Find("Manager");
        this.levelManager = manager.GetComponent<LevelManager>();
        this.battleMgr = manager.GetComponent<BattleManager>();
		
        Land = GameObject.FindGameObjectsWithTag("Land");
		attackButton = GameObject.Find ("Attack");

        // Find out who the opponent is and set the string
        opponent = gameObject.tag == GameConstants.TAG_ENEMY ? GameConstants.TAG_PLAYER : GameConstants.TAG_ENEMY;

		enemies = GameObject.FindGameObjectsWithTag (opponent);
    }

	void Update () {

	}

	void FixedUpdate() {
		this.setWalkableLand ();
	}

    void OnMouseDown()
    {
		// Selecting a character on your team, unless in the middle of an action
        if (this.levelManager.IsTurn(this.gameObject) && !this.levelManager.inMiddleOfTurn())
            this.activateSelectedCharacter();

        // Selecting a character to attack
        else if (levelManager.ActiveCharacter != null && levelManager.ActiveCharacterCtrl.IsAttacking() && !levelManager.IsTurn(this.gameObject))
            levelManager.ActiveCharacterCtrl.FinalizeAttack(this);
       
        // Select a character to view
        else
            this.levelManager.SetSelectedCharacter(this.gameObject);
    }

    private void activateSelectedCharacter()
    {
        this.levelManager.SetActiveCharacter(this.gameObject);
        this.levelManager.SetSelectedCharacter(this.gameObject);
    }

    public void Move(Vector3 location)
    {
		this.gameObject.GetComponentInParent<NavScript> ().moveAgent (location);
        this.HasMoved = true;
        this.levelManager.CheckTurnEnd();
    }

    public bool IsAttacking()
    {
        return this.isAttacking;
    }

    public void StartAttack()
    {
        this.isAttacking = true;
    }

    public void FinalizeAttack(CharacterController attackee)
    {
        if (!this.HasAttacked)
        {
            this.isAttacking = false;
            this.HasAttacked = true;
            this.battleMgr.DoBattle(this, attackee);
            this.levelManager.EndAttackSequence();
            this.levelManager.ResetCharColor();
            this.levelManager.CheckTurnEnd();
        }
    }

    public void ForfeitMovement()
    {
        this.HasMoved = false;
        this.levelManager.CheckTurnEnd();
    }

    public void ForfeitAttack()
    {
        this.isAttacking = false;
        this.HasAttacked = true;
        this.levelManager.EndAttackSequence();
        this.levelManager.ResetCharColor();
        this.levelManager.CheckTurnEnd();
    }

    public void StopAttack()
    {
        this.isAttacking = false;
    }

    public void UpdateEnemiesInRange()
    {
        enemiesInRange = gameObject.GetComponent<AttackRangeScript>().getObjectsInRadius(opponent);
    }


	public void Rest() {
		if(!this.HasAttacked) {
            this.HP = Mathf.Min(this.HP + 2, this.character.health);
			this.HasAttacked = true;
            this.HasMoved = true;
            this.levelManager.CheckTurnEnd();
		}
	}

    public bool IsInitialized()
    {
        return this.initialized;
    }

	public void setWalkableLand () {
		GameObject [] temp;
		int count = 0;

		for (int i = 0; i < Land.Length; i++) {
			if (Land [i].GetComponent<LandScript> ().walkable == true)
				count++;
		}
		temp = new GameObject[count];
		for (int i = 0, j = 0; i < Land.Length; i++) {
			if (Land [i].GetComponent<LandScript> ().walkable == true)
				temp[j++] = Land[i];
		}
		walkableLand = temp;
	}

	public GameObject[] getWalkableLand() {
		return walkableLand;
	}

    public int GetMaxHP()
    {
        return this.character.health;
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
                return this.HP;
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
                this.HP = (int)value;
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

    public string GetCharacterName()
    {
        if (this.initialized)
            return this.character.Name;
        else
            return null;
    }

    public List<CharacterController> GetTeamMembers()
    {
        var teammates = this.levelManager.GetTeam(this.gameObject.tag);
        return teammates != null ? 
            teammates
            .Select(x => x.GetComponent<CharacterController>())
            .Where(y => y.character.status != Character.Status.DEAD)
            .ToList() : null;
    }

    public List<CharacterController> GetEnemyTeamMembers()
    {
        var teammates = this.levelManager.GetTeam(this.gameObject.tag == GameConstants.TAG_PLAYER ? GameConstants.TAG_ENEMY : this.gameObject.tag);
        return teammates != null ? 
            teammates
            .Select(x => x.GetComponent<CharacterController>())
            .Where(y => y.character.status != Character.Status.DEAD)
            .ToList() : null;
    }

    /// <summary>
    /// Initializes the script with a character.
    /// </summary>
    /// <param name="c"></param>
    public void SetCharacter(Character c)
    {
        this.character = c;
        this.HP = c.health;
        this.initialized = true;
    }

    public void ResetTurn()
    {
        this.HasMoved = this.HasAttacked = false;
    }

    #region Neural Network related activities

    public void AttachNetwork(INeuralNetwork attackNetwork)
    {
        this.attackNetwork = attackNetwork;
        this.hasNetworkAttached = true;
    }

    public bool IsControlledByAI()
    {
        return this.hasNetworkAttached;
    }

    public void AttachTrainer(INetworkTrainer trainer)
    {
        this.trainer = trainer;
        this.isTraining = true;
    }

    public void CheckTrainingInput()
    {
        if (this.isTraining)
            this.trainer.CheckTrainingInput();
    }

    public bool IsTraining()
    {
        return this.isTraining;
    }

    public void ShowTraining()
    {
        this.trainer.DisplayOutputs();
        this.trainer.DisplaySensors();
    }

    public void HideTraining()
    {
        this.trainer.HideOutputs();
        this.trainer.HideSensors();
    }

    #endregion
}