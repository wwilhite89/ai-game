using UnityEngine;
using System.Collections;
using GameDB;
using GameDB.SessionData;

public class PlayerScript : MonoBehaviour {

	public bool isActive;
	public bool pieceLeftToMove;

	public Sprite[] playerSprites;
    private Character character;
	public GameObject[] enemiesInRange;
	private string opponent;

	// Use this for initialization
	void Start () {
		isActive = false;
        pieceLeftToMove = true;

		// Find out who the opponent is and set the string
		if (gameObject.tag == GameConstants.TAG_ENEMY)
			opponent = GameConstants.TAG_PLAYER;
		else
            opponent = GameConstants.TAG_ENEMY;

		SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>(true);
		sr [0].sprite = playerSprites[0];
	}
	
	// Update is called once per frame
	void Update () {
		enemiesInRange = gameObject.GetComponent<AttackRangeScript> ().getObjectsInRadius (opponent);
	}

	void OnMouseDown() {

		this.GetComponentInParent<LevelManagerScript>().resetActive ();
		isActive = true;

	}

	void OnTriggerEnter( Collider col ) {

		if (this.gameObject.tag == "Player" && col.gameObject.tag == "Enemy") {
            gameObject.GetComponentInParent<LevelManagerScript>().attack();
		}
		if (this.gameObject.tag == "Enemy" && col.gameObject.tag == "Player") {
            gameObject.GetComponentInParent<LevelManagerScript>().attack();
		}
	}

    public int GetHealth()
    {
        return this.character.HP;
    }

    public int GetSpeed()
    {
        return this.character.MOV;
    }

    public int GetAttackRange()
    {
        return this.character.RANGE;
    }

    public void SetCharacter(Character c)
    {
        this.character = c;
    }

}
