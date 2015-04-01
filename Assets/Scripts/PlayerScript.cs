using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public int isActive;
	public bool pieceLeftToMove;
	public float moveSpeed;
	public int attack;
	public int defense;
	public int luck;
	public Sprite[] playerSprites;

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

}
