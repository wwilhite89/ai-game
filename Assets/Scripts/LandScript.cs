using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using GameDB.SessionData;
using System;

public class LandScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    private GameObject enemy;
    private GameObject currentChar;
    private GameObject levelManager;
    public string LandType;
	private GameObject [] enemies;
	private GameObject[] characters;
    private Color highlighColor;
    private Vector3 blockPosition;
    private float playerDist;
    private Color landColor;
    private LevelManager lvlMgr;
	public bool walkable;

    // Modifies movement speed of characters. 1 is normal, 0 is unwalkable.
    public float speed;
    private bool selectable = false;
    private bool hovering = false;
    private Color lastColor;

    private CharacterController lastPlayer = null;

    // Use this for initialization
    void Start()
    {
        this.lvlMgr = GameObject.Find("Manager").GetComponent<LevelManager>();

        if (renderer.material.HasProperty("_Color"))
        {
            landColor = renderer.material.color;
            highlighColor = new Color(255, renderer.material.color.g, renderer.material.color.b);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		enemies = lvlMgr.getEnemies ();
		characters = lvlMgr.getPlayers ();
        if (lvlMgr.CurrentTurn == LevelManager.Turn.PLAYER)
        {
            if (this.lastPlayer != lvlMgr.ActiveCharacterCtrl || 
                (this.lastPlayer != null && this.lastPlayer.HasMoved && walkable))
            {
                this.selectable = LandCheck();
                if (!this.selectable) this.hovering = false;
                HighlightLand(this.selectable);
                this.lastPlayer = lvlMgr.ActiveCharacterCtrl;
            }
        }
        else
        {
            this.selectable = false;
            this.hovering = false;
            if (this.lastPlayer != null)
                HighlightLand(false);
            this.lastPlayer = null;
        }
    }

	#region IPointerEnterHandler implementation

	public void OnPointerEnter (PointerEventData eventData)
	{
		if (selectable)
		{
			this.hovering = true;
			this.lastColor = renderer.material.color;
			renderer.material.color = new Color(100f/255f,149f/255f,237f/255f);
		}
	}

	#endregion

	#region IPointerExitHandler implementation

	public void OnPointerExit (PointerEventData eventData)
	{
		if (selectable)
		{
			this.hovering = false;
			renderer.material.color = lastColor;
		}
	}

	#endregion

	public void OnPointerClick (PointerEventData eventData)
	{
		var player = lvlMgr.ActiveCharacter;

        // Land must be walkable
        if (lvlMgr.CurrentTurn == LevelManager.Turn.PLAYER && player != null && gameObject.GetComponent<LandScript>().speed != 0)
        {
            blockPosition = gameObject.transform.position;
            blockPosition.y += .6f;

            playerDist = Vector3.Distance(gameObject.transform.position, player.transform.position);

            Debug.Log("you clicked distance " + playerDist);
            
            if (playerDist < player.GetComponent<CharacterController>().GetStat(GameDB.Character.Stats.MOV) && !player.GetComponent<CharacterController>().HasMoved && lvlMgr.IsTurn(player))
            {
                player.GetComponent<CharacterController>().Move(blockPosition);
            }
        }
	}

	bool LandCheck() {
		var player = lvlMgr.ActiveCharacter;
		float dist;
		walkable = false;

        // Can the current player even move?
        if (player == null || lvlMgr.ActiveCharacterCtrl.HasMoved)
        {
            walkable = false;
            return false;
        }

		// is there an enemy on the land tile
		for (int i = 0; i < enemies.Length; i++) {
            if (enemies[i].transform.position.x == this.transform.position.x
                && enemies[i].transform.position.z == this.transform.position.z)
                return false;
		}
		// is there a character on the land tile
		for (int i = 0; i < characters.Length; i++) {
            if (characters[i].transform.position.x == this.transform.position.x
                && characters[i].transform.position.z == this.transform.position.z)
                return false;
		}

		if (player != null && !player.GetComponent<CharacterController>().HasMoved)
		{
			dist = Vector3.Distance(gameObject.transform.position, player.transform.position);
			
			// highlight the land around a selected player if he is active and able to move
			if (dist < player.GetComponent<CharacterController>().GetStat(GameDB.Character.Stats.MOV) && gameObject.GetComponent<LandScript>().speed != 0)
			{
					walkable = true;
					return true;
			}
		}

		walkable = false;
		return false;
	}

    void HighlightLand( bool b)
    {
        if (hovering)
            this.lastColor = b ? highlighColor : landColor;
        else
            this.renderer.material.color = b ? highlighColor : landColor;
    }

    // is called every frame on objects that are colliding
    void OnCollisionStay(Collision col)
    {
    
    }
}
