using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using GameDB.SessionData;
using System;

public class LandScript : MonoBehaviour, IPointerClickHandler {
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

    // Modifies movement speed of characters. 1 is normal, 0 is unwalkable.
    public float speed;

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
    void Update()
    {
		enemies = lvlMgr.getEnemies ();
		characters = lvlMgr.getPlayers ();
        HighlightLand(LandCheck());
    }

	public void OnPointerClick (PointerEventData eventData)
	{
		var player = lvlMgr.ActiveCharacter;

        // Land must be walkable
        if (player != null && gameObject.GetComponent<LandScript>().speed != 0)
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
		// is there an enemy on the land tile
		for (int i = 0; i < enemies.Length; i++) {
			if ( enemies[i].transform.position.x == this.transform.position.x
			    && enemies[i].transform.position.z == this.transform.position.z)
				return false;
		}
		// is there a character on the land tile
		for (int i = 0; i < characters.Length; i++) {
			if ( characters[i].transform.position.x == this.transform.position.x
			    && characters[i].transform.position.z == this.transform.position.z)
				return false;
		}

		if (player != null)
		{
			dist = Vector3.Distance(gameObject.transform.position, player.transform.position);
			
			// highlight the land around a selected player if he is active and able to move
			if (dist < player.GetComponent<CharacterController>().GetStat(GameDB.Character.Stats.MOV) && gameObject.GetComponent<LandScript>().speed != 0)
			{
				if (!player.GetComponent<CharacterController>().HasMoved)
					return true;
			}
		}
		return false;
	}

    void HighlightLand( bool b)
    {
        this.renderer.material.color = b ? highlighColor : landColor;
    }



    // is called every frame on objects that are colliding
    void OnCollisionStay(Collision col)
    {
    
		Debug.Log(col.gameObject.name);
         
    }
}
