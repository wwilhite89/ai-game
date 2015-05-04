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

    private Queue<Message> messageQueue = new Queue<Message>();
    private bool guiInitialized = false;
    private Rect messageBanner;
    private GUIStyle style;
    private Color guiColor = Color.white;
    private System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
    private int showForTime = 0;
    private string currentMessage;

    public Text next;

    public bool ControlsEnabled {get; private set;}
    private bool isAttacking = false;
    private NavScript navigation = null;

    private Queue<CharacterController> pendingAiQueue = new Queue<CharacterController>();

    private AudioClip restingClip;
    private AudioClip missClip;
    private AudioClip hitClip;

    internal class Message
    {
        public string content;
        public Color color;
    }

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
        this.messageBanner = new Rect(0, -50 + Screen.height / 2, Screen.width, 100);
        this.ControlsEnabled = true;
        this.EnqueueMessage("Player Turn", Color.white);
        this.restingClip = Resources.Load(@"Sound/rest") as AudioClip;
        this.missClip = Resources.Load(@"Sound/miss") as AudioClip;
        this.hitClip = Resources.Load(@"Sound/hit") as AudioClip;
	}

	void Update() {
		if(ActiveCharacter != null) {
            if (ActiveCharacterCtrl.IsTraining())
                ActiveCharacterCtrl.CheckTrainingInput();
		}
	}

    void OnGUI()
    {
        if (!this.guiInitialized)
        {
            style = GUI.skin.box;
            style.fontSize = 30;
            style.fontStyle = FontStyle.BoldAndItalic;
            style.alignment = TextAnchor.MiddleCenter;
            this.guiInitialized = true;
            this.showForTime = 2;
        }

        // Show any current messages
        if (this.currentMessage != null)
        {
            GUI.color = guiColor;
            // dodge, dealt, rests
            GUI.Box(this.messageBanner, this.currentMessage, this.style);
        }

        // Stop the current message
        if (this.watch.IsRunning && (this.watch.ElapsedMilliseconds / 1000f) > this.showForTime)
        {
            this.currentMessage = null;
            this.watch.Stop();
            this.watch.Reset();
        }

        // Queue the next message
        if (!this.watch.IsRunning && this.messageQueue.Count > 0)
        {
            var msg = this.messageQueue.Dequeue();
            this.currentMessage = msg.content;

            bool dmg = this.currentMessage.Contains("dealt"),
                rest = this.currentMessage.Contains("rests"),
                miss = this.currentMessage.Contains("dodges");

            if (dmg || rest || miss)
            {
                var audio = this.gameObject.GetComponent<AudioSource>();
                audio.clip = dmg ? hitClip : (rest ? restingClip : missClip);
                audio.Play();
                //audio.PlayOneShot(dmg ? this.hitClip : rest ? this.restingClip : this.missClip);
            }

            this.guiColor = msg.color;
            this.watch.Start();
        }
        
    }

    public void SetMovementAgent(NavScript navigation)
    {
        this.navigation = navigation;
    }

    public bool IsMoving()
    {
        if (this.navigation == null)
            return false;

        return this.navigation.IsMoving();
    }

    public int GetMessageQueueCount()
    {
        return this.messageQueue.Count;
    }

    public void EnqueueMessage(string message, Color color)
    {
        this.messageQueue.Enqueue(new Message { content = message, color = color });
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
        {
            this.EnqueueMessage("No enemies in range to attack.", Color.red);
            this.EndAttackSequence();
        }
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
        // return GameObject.FindGameObjectsWithTag(GameConstants.TAG_ENEMY);
        return enemies;
	}

	public GameObject[] getPlayers() {
        // return GameObject.FindGameObjectsWithTag(GameConstants.TAG_PLAYER);
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
        this.messageQueue.Enqueue(new Message { content = (this.CurrentTurn == Turn.ENEMY ? "Enemy" : "Player") + " Turn", color = Color.white });

        if (this.CurrentTurn == Turn.ENEMY)
            this.performEnemyTurn();
    }

    private void performEnemyTurn()
    {
        this.ControlsEnabled = false;
        //this.timer = new System.Timers.Timer(2);
        //timer.Elapsed += enemyTurnTimeElapsed;

        foreach (var enemy in this.enemies)
        {
            var controller = enemy.GetComponent<CharacterController>();

            if (controller.IsControlledByAI())
                this.pendingAiQueue.Enqueue(controller);
        }

        //this.timer.Start();
        StartCoroutine(this.executeEnemyActions());
    }

    private IEnumerator executeEnemyActions()
    {
        yield return new WaitForSeconds(4); // Wait for turn banners

        StartCoroutine(executeSingleEnemyAction());

        this.ControlsEnabled = true;
    }

    private IEnumerator executeSingleEnemyAction()
    {

        while (this.pendingAiQueue.Count > 0)
        {

            var nextAI = this.pendingAiQueue.Dequeue();

            // Activate the character
            this.SetSelectedCharacter(nextAI.gameObject);
            this.SetActiveCharacter(nextAI.gameObject);

            nextAI.attackNetwork.Sense();

            var decision = ArtificialNeuralNetworks.AttackNetwork.AttackNetwork.GetDecision(nextAI.attackNetwork.Think());
            var msgs = 0;
            // Debug.Log(controller.GetCharacterName() + " decides to " + decision.ToString());
            nextAI.GetComponent<AIChoiceScript>().MoveAI(decision, out msgs);

            // Resume
            yield return new WaitForSeconds(Mathf.Max(1.5f, msgs * 2f)); // 2 secs for each msg
            //yield return this.executeEnemyActions();
        }

    }

    private void performSingleEnemyAction(GameObject currentEnemy, GameObject[] remainder)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
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
        {
            
            characters[i].renderer.material.color = Color.green;
        }
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
        var temp = 0;
		ActiveCharacterCtrl.GetComponent<AIChoiceScript>().MoveAI (decision, out temp);
	}

    #endregion
}
