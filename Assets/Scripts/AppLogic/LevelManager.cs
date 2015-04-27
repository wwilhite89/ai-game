using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GameDB;
using GameDB.SessionData;
using ArtificialNeuralNetworks.AttackNetwork;
using System.Linq;

/// <summary>
/// Singleton class that manages the actions of the Level, including players' turns.
/// </summary>
public class LevelManager : MonoBehaviour {

    public enum Turn { PLAYER, ENEMY }

    public Turn CurrentTurn { get; private set; }
    public GameObject ActiveCharacter { get; private set; }
    public GameObject SelectedCharacter;
	public CharacterController ActiveCharacterCtrl;
    public CharacterController SelectedCharacterCtrl;
    private GameObject[] characters;
	private GameObject[] enemies;
    private GameManager gameManager;

    public Text next;

    private Rect turnBanner;
    private bool displayTurnBanner;
    public bool ControlsEnabled {get; private set;}
    private bool isAttacking = false;

    #region Public Methods

	void Start() {
		gameManager = new GameManager();
        this.characters = null;
        this.enemies = null;
        this.CurrentTurn = Turn.PLAYER;
        this.ActiveCharacter = null;
        this.spawnCharacters();
        this.ResetCharColor();
        if (next != null)  next.gameObject.SetActive(false);
        turnBanner = new Rect(0, -50 + Screen.height / 2, Screen.width, 100);
        this.displayTurnBanner = true;
        this.ControlsEnabled = true;
	}

	void Update() {
		if(ActiveCharacter != null) {
            if (ActiveCharacterCtrl.IsTraining())
                ActiveCharacterCtrl.CheckTrainingInput();
		}
	}

    void OnGUI()
    {
        if (this.displayTurnBanner)
            StartCoroutine(this.showTurnBanner());
    }

    public bool inMiddleOfTurn()
    {
        if (ActiveCharacterCtrl == null)
            return false;

        // Completed turn
        if (this.ActiveCharacterCtrl.HasAttacked && this.ActiveCharacterCtrl.HasMoved)
            return false;

        // Has performed one action, but not the other
        return this.ActiveCharacterCtrl.HasAttacked ^ this.ActiveCharacterCtrl.HasMoved;
    }

    public bool IsAttacking()
    {
        return this.isAttacking;
    }

    public void BeginAttackSequence()
    {
        this.isAttacking = true;

        if (this.ActiveCharacterCtrl == null)
            return;

        this.ActiveCharacterCtrl.UpdateEnemiesInRange();

        if (this.ActiveCharacterCtrl.enemiesInRange.Length > 0)
        {
            ActiveCharacterCtrl.StartAttack();
            this.HighlightEnemies();
        }
        else
            this.EndAttackSequence();
    }

    public void EndAttackSequence()
    {
        this.isAttacking = false;
    }

    public GameObject[] GetTeam(string team)
    {
        if (team == GameConstants.TAG_ENEMY)
            return this.enemies;
        else if (team == GameConstants.TAG_PLAYER)
            return this.characters;
        return null;
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
        // Hide training for previous character
        this.ResetCharColor();

        if (ActiveCharacterCtrl != null && ActiveCharacterCtrl.IsTraining())
        {
            ActiveCharacterCtrl.HideTraining();
        }

        this.ActiveCharacter = character;

        if (character != null)
        {
            this.ActiveCharacterCtrl = character.GetComponent<CharacterController>();
            this.ActiveCharacterCtrl.renderer.material.color = Color.cyan;
            this.ActiveCharacterCtrl.setWalkableLand();
            // Show training for new selected character
            if (ActiveCharacterCtrl != null && ActiveCharacterCtrl.IsTraining())
                ActiveCharacterCtrl.ShowTraining();
        }
    }

    public void SetSelectedCharacter(GameObject character)
    {
        this.SelectedCharacter = character;
        if (character != null)
        {
            this.SelectedCharacterCtrl = character.GetComponent<CharacterController>();
        }
    }

	public GameObject[] getEnemies() {
		return enemies;
	}

	public GameObject[] getPlayers() {
		return characters;
	}
	
    public bool IsTurn(GameObject player)
    {
        if (player.tag == GameConstants.TAG_ENEMY)
            return this.CurrentTurn == Turn.ENEMY;
        return this.CurrentTurn == Turn.PLAYER;
    }

    public void CheckTurnEnd()
    {
        if (this.ActiveCharacterCtrl != null && ActiveCharacterCtrl.HasMoved && ActiveCharacterCtrl.HasAttacked)
            this.ResetCharColor();

        var characters = this.CurrentTurn == Turn.ENEMY ? this.enemies : this.characters;
        var end = characters.Count() == 0 || characters.Count(x =>
            {
                var p = x.GetComponent<CharacterController>();
                return !p.HasAttacked || !p.HasMoved;
            }) == 0;

        if (end)
            this.changeTurn();
    }
	
    private void changeTurn()
    {
        this.CurrentTurn = this.CurrentTurn == Turn.ENEMY ? Turn.PLAYER : Turn.ENEMY;
        var characters = this.CurrentTurn == Turn.ENEMY ? this.enemies : this.characters;
        characters.ToList().ForEach(x => x.GetComponent<CharacterController>().ResetTurn());
        // next.gameObject.SetActive(true);
        this.SetActiveCharacter(null);
        this.SetSelectedCharacter(null);
        this.displayTurnBanner = true;

        if (this.CurrentTurn == Turn.ENEMY)
            StartCoroutine(this.performEnemyTurn());
    }

    private IEnumerator performEnemyTurn()
    {
        this.ControlsEnabled = false;
        yield return new WaitForSeconds(5f);

        foreach (var enemy in this.enemies)
        {
            var controller = enemy.GetComponent<CharacterController>();

            if (controller.IsControlledByAI())
            {
                float waitTime = 5f;
                // Activate the character
                this.SetSelectedCharacter(enemy);
                this.SetActiveCharacter(enemy);
                controller.attackNetwork.Sense();
                var decision = ArtificialNeuralNetworks.AttackNetwork.AttackNetwork.GetDecision(controller.attackNetwork.Think());
                Debug.Log(string.Format("{0} decides to {1}", controller.GetCharacterName(), decision.ToString()));
                controller.GetComponent<AIChoiceScript>().MoveAI(decision);

                if (decision != AttackNetwork.DECISION.REST) waitTime += 3f;

                yield return new WaitForSeconds(waitTime);
            }
        }

        yield return new WaitForSeconds(5f);
        //this.ControlsEnabled = true;
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

	public void HighlightEnemies() {
		for (int i = 0; i < ActiveCharacterCtrl.enemiesInRange.Length; i++) {
			ActiveCharacterCtrl.enemiesInRange[i].renderer.material.color = Color.red;
		}
	}

	public void ResetCharColor() {
		for (int i = 0; i < characters.Length; i++)
			characters [i].renderer.material.color = Color.green;
		for (int i = 0; i < enemies.Length; i++)
			enemies [i].renderer.material.color = Color.black;
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

	public void manageAIMove (AttackNetwork.DECISION decision) {
		ActiveCharacterCtrl.GetComponent<AIChoiceScript>().MoveAI (decision);
	}

    private IEnumerator showTurnBanner()
    {
        GUIStyle style = GUI.skin.box;
        style.fontSize = 30;
        style.alignment = TextAnchor.MiddleCenter;
        GUI.Box(this.turnBanner, (this.CurrentTurn == Turn.ENEMY ? "Enemy" : "Player") + " Turn");
        yield return new WaitForSeconds(2.5f);
        this.displayTurnBanner = false;
        yield return new WaitForSeconds(0);
    }
    #endregion
}
