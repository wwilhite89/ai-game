using UnityEngine;
using System.Collections;
using GameDB;

public class PlayerScript : MonoBehaviour {
//TODO(wil) can this be removed since we have Database/Character.cs? 
	public int isActive;
	public bool pieceLeftToMove;
	public float moveSpeed;
	public int attack;
	public int defense;
	public int luck;
	public Sprite[] playerSprites;
    private Character character;

	// Use this for initialization
	void Start () {
		isActive = 0;
		pieceLeftToMove = true;
		SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>(true);
		sr [0].sprite = playerSprites[0];
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown() {

		this.GetComponentInParent<ParentPlayerScript>().resetActive ();
		isActive = 1;

	}

    public int GetHealth()
    {
        return this.character.HP;
    }
}
