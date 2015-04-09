using UnityEngine;
using System.Collections;
using GameDB;

public class PlayerScript : MonoBehaviour {
//TODO(wil) can this be removed since we have Database/Character.cs? 
	public int isActive;
	public bool pieceLeftToMove;
	public float moveSpeed;
	public int attack;
	public float attackRange;
	public int defense;
	public int health;
	public int luck;
	public Sprite[] playerSprites;
    private Character character;
	public GameObject[] enemiesInRange;
	private string opponent;

	// Use this for initialization
	void Start () {
		isActive = 0;

		// find out who the opponent is and set the string
		if (gameObject.tag == "Enemy") {
			opponent = "Player";
		} else if (gameObject.tag == "Player") {
			opponent = "Enemy";
		}

		pieceLeftToMove = true;
		SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>(true);
		sr [0].sprite = playerSprites[0];
	}
	
	// Update is called once per frame
	void Update () {
		enemiesInRange = gameObject.GetComponent<AttackRangeScript> ().getObjectsInRadius (opponent);
	}

	void OnMouseDown() {

		this.GetComponentInParent<ParentPlayerScript>().resetActive ();
		isActive = 1;

	}

	void OnTriggerEnter( Collider col ) {

		if (this.gameObject.tag == "Player" && col.gameObject.tag == "Enemy") {
			gameObject.GetComponentInParent<ParentPlayerScript>().attack();
		}
		if (this.gameObject.tag == "Enemy" && col.gameObject.tag == "Player") {
			gameObject.GetComponentInParent<ParentPlayerScript>().attack();
		}
	}

    public int GetHealth()
    {
        return this.character.HP;
    }
}
