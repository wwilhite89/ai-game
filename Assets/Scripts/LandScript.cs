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

    private Color highlighColor;
    private Vector3 blockPossition;
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
        HighlightLand();
    }

	public void OnPointerClick (PointerEventData eventData)
	{
		var player = lvlMgr.ActiveCharacter;

        // Land must be walkable
        if (player != null && gameObject.GetComponent<LandScript>().speed != 0)
        {
            blockPossition = gameObject.transform.position;
            blockPossition.y += .6f;

            playerDist = Vector3.Distance(gameObject.transform.position, player.transform.position);

            Debug.Log("you clicked distance " + playerDist);
            
            if (playerDist < player.GetComponent<CharacterController>().GetStat(GameDB.Character.Stats.MOV) && !player.GetComponent<CharacterController>().HasMoved && lvlMgr.IsTurn(player))
            {

                player.GetComponent<CharacterController>().moveCharacter(blockPossition);
                lvlMgr.CheckTurnEnd();
            }
        }
	}

    void HighlightLand()
    {
        float dist;
        var player = lvlMgr.ActiveCharacter;
        bool highlight = false;

        if (player != null)
        {
            dist = Vector3.Distance(gameObject.transform.position, player.transform.position);

            // highlight the land around a selected player if he is active and able to move
            if (dist < player.GetComponent<CharacterController>().GetStat(GameDB.Character.Stats.MOV) && gameObject.GetComponent<LandScript>().speed != 0)
            {
                if (!player.GetComponent<CharacterController>().HasMoved)
                    highlight = true;
            }
        }
        
        this.renderer.material.color = highlight ? highlighColor : landColor;
    }

    // is called every frame on objects that are colliding
    void OnCollisionStay(Collision col)
    {

        if (col.collider.tag == GameConstants.TAG_PLAYER || col.collider.tag == GameConstants.TAG_ENEMY)
        {
            this.renderer.material.color = landColor;
        }
    }
}
